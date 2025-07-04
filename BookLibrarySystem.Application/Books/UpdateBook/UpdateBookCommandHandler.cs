using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Description = BookLibrarySystem.Domain.Books.Description;

namespace BookLibrarySystem.Application.Books.UpdateBook;

public class UpdateBookCommandHandler : ICommandHandler<UpdateBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookGenreRepository _bookGenreRepository;
    private readonly ILogger<UpdateBookCommandHandler> _logger;

    public UpdateBookCommandHandler(
        IBookRepository bookRepository,
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork,
        IAuthorRepository authorRepository,
        ILogger<UpdateBookCommandHandler> logger,
        IBookGenreRepository bookGenreRepository)
    {
        _bookRepository = bookRepository;
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
        _authorRepository = authorRepository;
        _logger = logger;
        _bookGenreRepository = bookGenreRepository;
    }

    public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId, "Genres",
                cancellationToken: cancellationToken);
            if (book == null)
            {
                return Result.Failure<Book>(BookErrors.NotFound);
            }

            var updateDetailsResult = await UpdateBookDetailsAsync(book, request, cancellationToken);
            if (updateDetailsResult.IsFailure)
            {
                return updateDetailsResult;
            }

            var updateGenresResult = await UpdateBookGenresAsync(book, request.GenreIds, cancellationToken);
            if (updateGenresResult.IsFailure)
            {
                return updateGenresResult;
            }

            _bookRepository.Update(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency conflict occurred while updating the book.");
            return Result.Failure<Book>(new Error("ConcurrencyError",
                "The book was modified by another user. Please refresh and try again."));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating the book. Inner Exception: {InnerException}",
                ex.InnerException?.Message);
            return Result.Failure<Book>(new Error("DatabaseError", ex.InnerException?.Message ?? ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the book. Exception: {Exception}", ex.ToString());
            return Result.Failure<Book>(new Error("UpdateBookError", "An error occurred while updating the book."));
        }
    }

    private async Task<Result> UpdateBookDetailsAsync(Book book, UpdateBookCommand command,
        CancellationToken cancellationToken)
    {
        if (command.PublicationDate == default)
        {
            return Result.Failure<Book>(BookErrors.InvalidPublishDate);
        }

        var author = await _authorRepository.GetByIdAsync(command.AuthorId, cancellationToken: cancellationToken);
        if (author == null)
        {
            return Result.Failure<Book>(AuthorErrors.NotFound);
        }

        book.UpdateDetails(
            new Title(command.Title),
            new Description(command.Description),
            command.PublicationDate,
            command.Pages,
            command.AuthorId
        );

        if (book.IsAvailable != command.IsAvailable)
        {
            var availabilityResult = command.IsAvailable
                ? book.MarkAsAvailable()
                : book.MarkAsUnavailable();

            if (availabilityResult.IsFailure)
            {
                return Result.Failure<Book>(availabilityResult.Error);
            }
        }

        return Result.Success();
    }

    private async Task<Result> UpdateBookGenresAsync(Book book, List<Guid> newGenreIds,
        CancellationToken cancellationToken)
    {
        var currentGenreIds = book.Genres.Select(g => g.GenreId).ToList();

        var genresToRemove = book.Genres
            .Where(bg => !newGenreIds.Contains(bg.GenreId))
            .ToList();

        foreach (var genreToRemove in genresToRemove)
        {
            book.Genres.Remove(genreToRemove);
            await _bookGenreRepository.DeleteAsync(genreToRemove.Id, cancellationToken);
        }

        var genresToAdd = newGenreIds
            .Where(id => !currentGenreIds.Contains(id))
            .ToList();

        foreach (var genreId in genresToAdd)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId, cancellationToken: cancellationToken);
            if (genre == null)
            {
                return Result.Failure<Book>(GenreErrors.NotFound);
            }

            var bookGenre = new BookGenre(book.Id, genre.Id);
            book.Genres.Add(bookGenre);
            await _bookGenreRepository.AddAsync(bookGenre, cancellationToken);
        }

        return Result.Success();
    }
}
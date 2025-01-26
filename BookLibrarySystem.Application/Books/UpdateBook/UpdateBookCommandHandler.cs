using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
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
        ILogger<UpdateBookCommandHandler> logger, IBookGenreRepository bookGenreRepository)
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
            var book = await _bookRepository.GetByIdAsync(request.BookId,"Genres", cancellationToken: cancellationToken);
            if (book == null)
            {
                return Result.Failure<Book>(BookErrors.NotFound);
            }

            var updateDetailsResult = await UpdateBookDetailsAsync(book, request.BookDto, cancellationToken);
            if (updateDetailsResult.IsFailure)
            {
                return updateDetailsResult;
            }

            var updateGenresResult = await UpdateBookGenresAsync(book, request.BookDto.GenreIds, cancellationToken);
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

    private async Task<Result> UpdateBookDetailsAsync(Book book, UpdateBookDto bookDto,
        CancellationToken cancellationToken)
    {

        if (bookDto.PublicationDate == default)
        {
            return Result.Failure<Book>(new Error("ValidationError", "Invalid publication date."));
        }

        var author = await _authorRepository.GetByIdAsync(bookDto.AuthorId, cancellationToken: cancellationToken);
        if (author == null)
        {
            return Result.Failure<Book>(AuthorErrors.NotFound);
        }

        book.UpdateDetails(
            new Title(bookDto.Title),
            new Description(bookDto.Description),
            bookDto.PublicationDate,
            bookDto.Pages,
            bookDto.AuthorId
        );

        if (book.IsAvailable != bookDto.IsAvailable)
        {
            var availabilityResult = bookDto.IsAvailable
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
        // Get the current genre IDs associated with the book
        var currentGenreIds = book.Genres.Select(g => g.GenreId).ToList();

        // Find genres to remove
        var genresToRemove = book.Genres
            .Where(bg => !newGenreIds.Contains(bg.GenreId))
            .ToList();

        // Remove the genres from the book
        foreach (var genreToRemove in genresToRemove)
        {
            book.Genres.Remove(genreToRemove); // Remove from the collection
            await _bookGenreRepository.DeleteAsync(genreToRemove.Id,
                cancellationToken); // Explicitly delete from the database
        }

        // Find genres to add
        var genresToAdd = newGenreIds
            .Where(id => !currentGenreIds.Contains(id))
            .ToList();

        // Add the new genres to the book
        foreach (var genreId in genresToAdd)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId, cancellationToken: cancellationToken);
            if (genre == null)
            {
                return Result.Failure<Book>(GenreErrors.NotFound);
            }

            var bookGenre = new BookGenre(book.Id, genre.Id); // Create a new BookGenre entity
            book.Genres.Add(bookGenre); // Add to the collection
            await _bookGenreRepository.AddAsync(bookGenre, cancellationToken); // Explicitly add to the database
        }

        return Result.Success();
    }
}
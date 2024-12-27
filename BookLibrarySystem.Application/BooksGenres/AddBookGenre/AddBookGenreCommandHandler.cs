using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.BooksGenres.AddBookGenre;

public class AddBookGenreCommandHandler : ICommandHandler<AddBookGenreCommand>
{
    private readonly IBookGenreRepository _bookGenreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBookGenreCommandHandler(IBookGenreRepository bookGenreRepository, IUnitOfWork unitOfWork)
    {
        _bookGenreRepository = bookGenreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddBookGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validation Ensure BookId and GenreId are not empty
            if (request.BookId == Guid.Empty)
            {
                return Result.Failure(BookGenreErrors.InvalidBookId);
            }

            if (request.GenreId == Guid.Empty)
            {
                return Result.Failure(BookGenreErrors.InvalidGenreId);
            }

            // Check if the BookGenre already exists
            var existingBookGenres = await _bookGenreRepository.GetByBookIdAsync(request.BookId ,cancellationToken);
            if (existingBookGenres.Any(bg => bg != null && bg.GenreId == request.GenreId))
            {
                return Result.Failure(BookGenreErrors.DuplicateGenre);
            }

            // Create a new BookGenre and add it
            var bookGenre = new BookGenre(request.BookId, request.GenreId);
            await _bookGenreRepository.AddAsync(bookGenre ,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(bookGenre);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure(BookGenreErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}
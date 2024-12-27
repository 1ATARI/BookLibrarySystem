using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.BooksGenres;

namespace BookLibrarySystem.Application.BooksGenres.DeleteBookGenre;

public class DeleteBookGenreCommandHandler : ICommandHandler<DeleteBookGenreCommand>
{
    private readonly IBookGenreRepository _bookGenreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookGenreCommandHandler(IBookGenreRepository bookGenreRepository, IUnitOfWork unitOfWork)
    {
        _bookGenreRepository = bookGenreRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<Result> Handle(DeleteBookGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if the BookGenre exists
            var bookGenre = await _bookGenreRepository.GetByIdAsync(request.BookGenreId, cancellationToken);
            if (bookGenre == null)
            {
                return Result.Failure(BookGenreErrors.NotFound);
            }

            // Delete the BookGenre
            await _bookGenreRepository.DeleteAsync(request.BookGenreId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
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
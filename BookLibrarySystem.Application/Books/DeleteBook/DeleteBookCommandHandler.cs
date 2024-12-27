using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.DeleteBook;

public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _bookRepository.ExistsAsync(request.BookId , cancellationToken);

            if (!exists)
            {
                return Result.Failure(BookErrors.NotFound);
            }

            var isDeleted = await _bookRepository.DeleteAsync(request.BookId , cancellationToken);

            if (!isDeleted)
            {
                return Result.Failure(BookErrors.DeleteFailed);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ConcurrencyException)
        {
            return Result.Failure(BookErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}
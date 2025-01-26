using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.DeleteUserBook;

internal sealed class DeleteUserBookCommandHandler : ICommandHandler<DeleteUserBookCommand, Result>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserBookCommandHandler(IUserBookRepository userBookRepository, IUnitOfWork unitOfWork)
    {
        _userBookRepository = userBookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(DeleteUserBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _userBookRepository.ExistsByIdAsync(request.UserBookId,cancellationToken);
            if (!exists)
            {
                return Result.Failure(UserBookErrors.UserBookNotFound);
            }

            await _userBookRepository.DeleteAsync(request.UserBookId,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ConcurrencyException)
        {
            return Result.Failure(UserBookErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}
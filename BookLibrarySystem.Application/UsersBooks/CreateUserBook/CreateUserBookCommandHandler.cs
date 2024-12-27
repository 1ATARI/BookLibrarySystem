using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.CreateUserBook;

internal sealed class CreateUserBookCommandHandler : ICommandHandler<CreateUserBookCommand, UserBook>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserBookCommandHandler(IUserBookRepository userBookRepository, IUnitOfWork unitOfWork)
    {
        _userBookRepository = userBookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserBook>> Handle(CreateUserBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.BorrowedDate > DateTime.Now)
            {
                return Result.Failure<UserBook>(UserBookErrors.InvalidBorrowedDate);
            }

            var userBook = UserBook.Borrow(request.UserId, request.BookId, request.BorrowedDate);

            await _userBookRepository.AddAsync(userBook,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return userBook;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<UserBook>(UserBookErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserBook>(new Error("DatabaseError", ex.Message));
        }
    }
}
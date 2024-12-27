using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Application.UsersBooks.BorrowUserBook;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.UsersBooks;

internal sealed class BorrowBookCommandHandler : ICommandHandler<BorrowBookCommand, Guid>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BorrowBookCommandHandler(
        IUserBookRepository userBookRepository,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _userBookRepository = userBookRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId ,cancellationToken);
            if (book == null || !book.IsAvailable)
            {
                return Result.Failure<Guid>(BookErrors.BookUnavailable);
            }

            if (request.BorrowedDate > DateTime.Now)
            {
                return Result.Failure<Guid>(UserBookErrors.InvalidBorrowedDate);
            }

            var userBook = UserBook.Borrow(request.UserId, request.BookId, request.BorrowedDate);

            var markAsUnavailableResult = book.MarkAsUnavailable();
            if (markAsUnavailableResult.IsFailure)
            {
                return Result.Failure<Guid>(markAsUnavailableResult.Error);
            }

            await _userBookRepository.AddAsync(userBook,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(userBook.Id);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(UserBookErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(new Error("DatabaseError", ex.Message));
        }
    }
}

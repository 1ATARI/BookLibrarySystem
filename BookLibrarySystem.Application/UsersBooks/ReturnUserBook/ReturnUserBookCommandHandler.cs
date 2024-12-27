using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.ReturnUserBook;

internal sealed class ReturnBookCommandHandler : ICommandHandler<ReturnBookCommand, Result>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReturnBookCommandHandler(
        IUserBookRepository userBookRepository,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _userBookRepository = userBookRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userBook = await _userBookRepository.GetByIdAsync(request.UserBookId,cancellationToken);
            if (userBook == null)
            {
                return Result.Failure(UserBookErrors.UserBookNotFound);
            }

            if (request.ReturnedDate < userBook.BorrowedDate)
            {
                return Result.Failure(UserBookErrors.InvalidReturnDate);
            }

            userBook.Return(request.ReturnedDate);

            var book = await _bookRepository.GetByIdAsync(userBook.BookId ,cancellationToken);
            if (book == null)
            {
                return Result.Failure(BookErrors.NotFound);
            }

            var markAsAvailableResult = book.MarkAsAvailable();
            if (markAsAvailableResult.IsFailure)
            {
                return Result.Failure(markAsAvailableResult.Error);
            }

            await _userBookRepository.UpdateAsync(userBook ,cancellationToken);
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
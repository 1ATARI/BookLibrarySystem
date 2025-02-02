using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.UpdateUserBook;

internal sealed class UpdateUserBookCommandHandler : ICommandHandler<UpdateUserBookCommand, Guid>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IApplicationUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserBookCommandHandler(IUserBookRepository userBookRepository, IUnitOfWork unitOfWork,
        IApplicationUserRepository userRepository, IBookRepository bookRepository)
    {
        _userBookRepository = userBookRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }

    public async Task<Result<Guid>> Handle(UpdateUserBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userBook = await _userBookRepository.GetByIdAsync(request.UserBookId, null, cancellationToken);
            if (userBook == null)
            {
                return Result.Failure<Guid>(UserBookErrors.UserBookNotFound);
            }

            var user = await _userRepository.GetByIdAsync(request.UserBookRequest.UserId, cancellationToken: cancellationToken);
            if (user == null)
            {
                return Result.Failure<Guid>(UserBookErrors.UserNotFound);
            }

            if (user.Id != userBook.UserId)
            {
                userBook.UpdateUserId(request.UserBookRequest.UserId);
            }

            if (request.UserBookRequest.BookId != userBook.BookId)
            {
                var newBook = await _bookRepository.GetByIdAsync(request.UserBookRequest.BookId, cancellationToken: cancellationToken);
                if (newBook == null || !newBook.IsAvailable)
                {
                    return Result.Failure<Guid>(BookErrors.BookUnavailable);
                }

                var oldBook = await _bookRepository.GetByIdAsync(userBook.BookId, cancellationToken: cancellationToken);
                if (oldBook != null)
                {
                    oldBook.MarkAsAvailable();
                    _bookRepository.Update(oldBook, cancellationToken);
                }

                newBook.MarkAsUnavailable();
                _bookRepository.Update(newBook, cancellationToken);

                // Update the book ID in the UserBook entity
                userBook.UpdateBookId(request.UserBookRequest.BookId);
            }

            // Update the borrowed date
            var updateBorrowedDateResult = userBook.UpdateBorrowedDate(request.UserBookRequest.BorrowedDate);
            if (updateBorrowedDateResult.IsFailure)
            {
                return Result.Failure<Guid>(updateBorrowedDateResult.Error);
            }

            // Handle returned date
            if (request.UserBookRequest.ReturnedDate.HasValue)
            {
                var updateReturnedDateResult = userBook.Return(request.UserBookRequest.ReturnedDate.Value);
                if (updateReturnedDateResult.IsFailure)
                {
                    return Result.Failure<Guid>(updateReturnedDateResult.Error);
                }

                // Mark the book as available if it's returned
                var currentBook = await _bookRepository.GetByIdAsync(userBook.BookId, cancellationToken: cancellationToken);
                if (currentBook != null)
                {
                    currentBook.MarkAsAvailable();
                    _bookRepository.Update(currentBook, cancellationToken);
                }
            }

            // Save changes
            _userBookRepository.Update(userBook, cancellationToken);
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
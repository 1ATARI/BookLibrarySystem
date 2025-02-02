using BookLibrarySystem.Application.Abstractions.Email;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.ReturnUserBook;

internal sealed class ReturnBookCommandHandler : ICommandHandler<ReturnBookCommand, Result>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IApplicationUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public ReturnBookCommandHandler(
        IUserBookRepository userBookRepository,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork, IApplicationUserRepository userRepository, IEmailService emailService)
    {
        _userBookRepository = userBookRepository;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<Result<Result>> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userBook = await _userBookRepository.GetByIdAsync(request.UserBookId,null, cancellationToken);
            if (userBook == null)
            {
                return Result.Failure(UserBookErrors.UserBookNotFound);
            }
            var user = await _userRepository.GetByIdAsync(userBook.UserId,null, cancellationToken);

            if (request.ReturnedDate < userBook.BorrowedDate)
            {
                return Result.Failure(UserBookErrors.InvalidReturnDate);
            }
            if (userBook.ReturnedDate.HasValue)
            {
                return Result.Failure(UserBookErrors.BookAlreadyReturned);
            }
            userBook.Return(request.ReturnedDate);

            var book = await _bookRepository.GetByIdAsync(userBook.BookId, cancellationToken: cancellationToken);
            if (book == null)
            {
                return Result.Failure(BookErrors.NotFound);
            }


            var markAsAvailableResult = book.MarkAsAvailable();
            if (markAsAvailableResult.IsFailure)
            {
                return Result.Failure(markAsAvailableResult.Error);
            }

            _userBookRepository.Update(userBook , cancellationToken: cancellationToken);
            var subject = "Book Borrowed Successfully";
            var message = $"Hello {user.Name.FirstName},\n\n" +
                          $"You have successfully borrowed the book '{book.Title}'.\n" +
                          $"Borrowed Date: {userBook.BorrowedDate:yyyy-MM-dd}\n\n" +
                          "Thank you for using our library!\n\n" +
                          "Best regards,\n" +
                          "Book Library Team";

             await _emailService.SendAsync(user.Email, subject, message);


      
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
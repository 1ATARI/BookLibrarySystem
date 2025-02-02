using BookLibrarySystem.Application.Abstractions.Email;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Application.UsersBooks.BorrowUserBook;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;

internal sealed class BorrowBookCommandHandler : ICommandHandler<BorrowBookCommand, Guid>
{
    private readonly IUserBookRepository _userBookRepository;
    private readonly IApplicationUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IEmailService _emailService;

    private readonly IUnitOfWork _unitOfWork;

    public BorrowBookCommandHandler(
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

    public async Task<Result<Guid>> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(request.BorrowBookRequest.BookId, cancellationToken: cancellationToken);
            if (book == null || !book.IsAvailable)
            {
                return Result.Failure<Guid>(BookErrors.BookUnavailable);
            }

            var user = await _userRepository.GetByIdAsync(request.BorrowBookRequest.UserId, cancellationToken: cancellationToken);
            if (user == null)
            {
                return Result.Failure<Guid>(UserBookErrors.UserNotFound);
            }
            
            if (request.BorrowBookRequest.BorrowedDate > DateTime.Now)
            {
                return Result.Failure<Guid>(UserBookErrors.InvalidBorrowedDate);
            }

            var userBook = UserBook.Borrow(request.BorrowBookRequest.UserId, request.BorrowBookRequest.BookId, request.BorrowBookRequest.BorrowedDate);

            var markAsUnavailableResult = book.MarkAsUnavailable();
            if (markAsUnavailableResult.IsFailure)
            {
                return Result.Failure<Guid>(markAsUnavailableResult.Error);
            }


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _userBookRepository.AddAsync(userBook,cancellationToken);
            var subject = "Book Borrowed Successfully";
            var message = $"Hello {user.Name.FirstName},\n\n" +
                          $"You have successfully borrowed the book '{book.Title}'.\n" +
                          $"Borrowed Date: {request.BorrowBookRequest.BorrowedDate:yyyy-MM-dd}\n\n" +
                          "Thank you for using our library!\n\n" +
                          "Best regards,\n" +
                          "Book Library Team";

            await _emailService.SendAsync(user.Email, subject, message);
 
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

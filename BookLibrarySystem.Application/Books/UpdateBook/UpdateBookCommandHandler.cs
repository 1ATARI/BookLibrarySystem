using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Books.UpdateBook;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.UpdateBooks;

public class UpdateBookCommandHandler : ICommandHandler<UpdateBookCommand, Book>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Book>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId , cancellationToken);

        if (book == null)
        {
            return Result.Failure<Book>(BookErrors.NotFound);
        }

        var title = new Title(request.Title);
        var description = new Description(request.Description);

        book.UpdateDetails(title, description, request.PublicationDate, request.Pages, request.AuthorId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return book;
    }
}
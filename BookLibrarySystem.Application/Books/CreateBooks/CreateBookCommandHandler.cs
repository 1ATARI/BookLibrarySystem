using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.CreateBooks;

public class CreateBookCommandHandler : ICommandHandler<CreateBookCommand ,Book>
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
 
        var title = new Title(request.Title);
        var description = new Description(request.Description);

        var book =   Book.Create(title, description, request.PublicationDate, request.Pages , request.AuthorId);
            
        await _bookRepository.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return book;
    }
}

using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;



public class AddBookToAuthorCommandHandler : ICommandHandler<AddBookToAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBookToAuthorCommandHandler(
        IAuthorRepository authorRepository,
        IUnitOfWork unitOfWork,
        IBookRepository bookRepository)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
    }

    public async Task<Result> Handle(AddBookToAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.AuthorId == Guid.Empty)
            {
                return Result.Failure(AuthorErrors.InvalidAuthorId);
            }

            var author = await _authorRepository.GetByIdAsync(request.AuthorId, cancellationToken: cancellationToken);
            if (author == null)
            {
                return Result.Failure(AuthorErrors.NotFound);
            }

            var title = new Title(request.Title);
            var description = new Description(request.Description);
            var book = Book.Create(title, description, request.PublicationDate, request.Pages, request.AuthorId);
            await _bookRepository.AddAsync(book, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (ConcurrencyException)
        {
            return Result.Failure(AuthorErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}

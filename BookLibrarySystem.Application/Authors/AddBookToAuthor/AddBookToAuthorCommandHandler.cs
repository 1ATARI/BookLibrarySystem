using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;

public class AddBookToAuthorCommandHandler : ICommandHandler<AddBookToAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddBookToAuthorCommandHandler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddBookToAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId,cancellationToken);

            if (author == null)
            {
                return Result.Failure(AuthorErrors.NotFound);
            }

            if (author.Books.Any(b => b.Id == request.Book.Id))
            {
                return Result.Failure(AuthorErrors.BookAlreadyAssigned);
            }

            author.AddBook(request.Book);

            await _authorRepository.UpdateAsync(author,cancellationToken);
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
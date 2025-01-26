using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.AddAuthor;

public class AddAuthorCommandHandler : ICommandHandler<AddAuthorCommand, Author>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddAuthorCommandHandler(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
    {
        _authorRepository = authorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var existingAuthor = await _authorRepository.GetAllAsync(
                filter: a => a.Name.FirstName == request.Name.FirstName && a.Name.LastName == request.Name.LastName,
                cancellationToken: cancellationToken);

            if (existingAuthor.Any())
            {
                return Result.Failure<Author>(AuthorErrors.AuthorAlreadyExists);
            }
            var author = Author.Create(request.Name);

            await _authorRepository.AddAsync(author, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(author);
        }

        catch (ConcurrencyException)
        {
            return Result.Failure<Author>(AuthorErrors.AuthorAlreadyExists);
        }

        catch (Exception ex)
        {
            return Result.Failure<Author>(new Error("DatabaseError", ex.Message));
        }
    }
}
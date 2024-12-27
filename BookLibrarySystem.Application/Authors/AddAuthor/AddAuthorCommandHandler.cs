using BookLibrarySystem.Application.Abstractions.Messaging;
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
        var existingAuthor = await _authorRepository.GetAllAsync(cancellationToken);
        if (existingAuthor.Any(a => a.Name.FirstName == request.Name.FirstName && a.Name.LastName == request.Name.LastName))
        {
            return Result.Failure<Author>(AuthorErrors.AuthorAlreadyExists);
        }

        var author = Author.Create(request.Name);

        await _authorRepository.AddAsync(author,cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(author);
    }
}
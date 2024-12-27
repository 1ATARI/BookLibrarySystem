using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.GetAuthorById;

public class GetAuthorByIdHandler : IQueryHandler<GetAuthorByIdQuery, Author>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAuthorByIdHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }


    public async Task<Result<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);

        if (author == null)
        {
            return Result.Failure<Author>(AuthorErrors.NotFound);
        }

        return author;
    }
}

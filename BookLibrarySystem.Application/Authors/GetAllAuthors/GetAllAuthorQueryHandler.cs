using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.GetAllAuthors;

public class GetAllAuthorQueryHandler : IQueryHandler<GetAllAuthorQuery, IEnumerable<Author>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAllAuthorQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    
    public async Task<Result<IEnumerable<Author>>> Handle(GetAllAuthorQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.GetAllAsync(cancellationToken);

        return Result.Success(authors);
        
        
        
    }
}
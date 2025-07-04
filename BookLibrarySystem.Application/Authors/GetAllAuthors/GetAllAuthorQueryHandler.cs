using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Models;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.GetAllAuthors;

public class GetAllAuthorQueryHandler : IQueryHandler<GetAllAuthorQuery, PagedResult<AuthorResponseDto>>
{
    private readonly IAuthorRepository _authorRepository;

    public GetAllAuthorQueryHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    
    public async Task<Result<PagedResult<AuthorResponseDto>>> Handle(
        GetAllAuthorQuery request,
        CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;

        var totalCount = await _authorRepository.GetCountAsync(cancellationToken :cancellationToken);
        var authors = await _authorRepository.GetAllAsync(
            skip: skip,
            take: request.PageSize,
            includeProperties: "Books",
            cancellationToken: cancellationToken);

        var authorDtos = authors.Select(a => new AuthorResponseDto(
            a.Id,
            a.Name.FirstName,
            a.Name.LastName,
            a.Books.Select(b => new BookResponseDto(
                b.Id,
                b.Title.Value,
                b.Description.Value,
                b.PublicationDate,
                b.Pages,
                b.IsAvailable)).ToList())).ToList();

        var pagedResult = new PagedResult<AuthorResponseDto>(
            authorDtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
        return Result.Success(pagedResult);
    }
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.GetAllAuthors;

public class GetAllAuthorQuery : IQuery<IEnumerable<AuthorResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
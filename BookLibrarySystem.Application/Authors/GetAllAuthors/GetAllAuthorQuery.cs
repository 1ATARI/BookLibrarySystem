using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.GetAllAuthors;

public class GetAllAuthorQuery : IQuery<IEnumerable<Author>>
{
}
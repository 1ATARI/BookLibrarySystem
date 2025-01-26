using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.UpdateAuthor;

public sealed record UpdateAuthorCommand(Guid Id , Name Name) : ICommand<Author>
{
    
}
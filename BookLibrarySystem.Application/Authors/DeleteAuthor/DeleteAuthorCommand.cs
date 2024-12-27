using BookLibrarySystem.Application.Abstractions.Messaging;

namespace BookLibrarySystem.Application.Authors.DeleteAuthor;

public sealed record DeleteAuthorCommand(Guid AuthorId) : ICommand;
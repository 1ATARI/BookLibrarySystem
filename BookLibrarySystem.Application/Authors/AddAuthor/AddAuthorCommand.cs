using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.AddAuthor;

public sealed record AddAuthorCommand(Name Name) : ICommand<Author>;
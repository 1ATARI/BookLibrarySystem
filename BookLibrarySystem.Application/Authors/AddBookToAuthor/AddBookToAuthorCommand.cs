using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;

public sealed record AddBookToAuthorCommand(Guid AuthorId, AddBookDto BookDto) : ICommand;

using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Authors;

namespace BookLibrarySystem.Application.Authors.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId) : IQuery<Author>;
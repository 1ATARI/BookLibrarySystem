using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByBookId;

public record GetUserBooksByBookIdQuery(Guid BookId) : IQuery<IEnumerable<UserBook>>;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByUserId;

public record GetUserBooksByUserIdQuery(Guid UserId) : IQuery<IEnumerable<UserBook>>;
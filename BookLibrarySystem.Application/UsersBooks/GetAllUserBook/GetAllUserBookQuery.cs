using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetAllUserBook;

public  sealed record GetAllUserBookQuery: IQuery<IEnumerable<UserBook>>;
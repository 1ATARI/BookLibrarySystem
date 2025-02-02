using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetAllUserBook;

public  sealed record GetAllUserBookQuery: IQuery<IEnumerable<UserBookDto>>{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.GetAllBooks;

public class GetAllBooksQuery : IQuery<IEnumerable<Book>>
{
}
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Domain.Authors;

public sealed class Author : Entity
{
    private Author(Guid id, Name name) : base(id)
    {
        Name = name;
        Books = new List<Book>();
    }

    private Author() { } // For EF Core

    public Name Name { get; private set; }
    public List<Book> Books { get; private set; }

    public static Author Create( Name name)
    {
        return new Author(Guid.NewGuid(), name);
    }
    
    public void AddBook(Book book)
    {
        Books.Add(book);
    }
}
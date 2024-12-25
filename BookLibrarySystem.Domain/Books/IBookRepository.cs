namespace BookLibrarySystem.Domain.Books;

public interface IBookRepository
{
    Book GetById(Guid bookId);
    IEnumerable<Book> GetAll();
    void Add(Book book);
    void Update(Book book);
    void Delete(Guid bookId);
    bool Exists(Guid bookId);
}
namespace BookLibrarySystem.Domain.BooksGenres;

public interface IBookGenreRepository
{
    BookGenre GetById(Guid bookGenreId);
    IEnumerable<BookGenre> GetAll();
    IEnumerable<BookGenre> GetByBookId(Guid bookId);
    IEnumerable<BookGenre> GetByGenreId(Guid genreId);
    void Add(BookGenre bookGenre);
    void Delete(Guid bookGenreId);
    bool Exists(Guid bookGenreId);
}
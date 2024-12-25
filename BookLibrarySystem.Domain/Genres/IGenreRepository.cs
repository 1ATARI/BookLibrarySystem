namespace BookLibrarySystem.Domain.Genres;

public interface IGenreRepository
{
    Genre GetById(Guid genreId);
    IEnumerable<Genre> GetAll();
    void Add(Genre genre);
    void Update(Genre genre);
    void Delete(Guid genreId);
    bool Exists(Guid genreId);
}
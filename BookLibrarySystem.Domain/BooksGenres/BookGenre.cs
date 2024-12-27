using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Domain.BooksGenres

{
    public sealed class BookGenre : Entity
    {
        public BookGenre(Guid bookId, Guid genreId)
            : base(Guid.NewGuid())
        {
            if (bookId == Guid.Empty) throw new ArgumentException("Invalid book ID.", nameof(bookId));
            if (genreId == Guid.Empty) throw new ArgumentException("Invalid genre ID.", nameof(genreId));

            BookId = bookId;
            GenreId = genreId;
        }

        private BookGenre() { } // For EF Core

        public Guid BookId { get; private set; }
        public Book Book { get; private set; }

        public Guid GenreId { get; private set; }
        public Genre Genre { get; private set; }

        public void UpdateGenre(Genre newGenre)
        {
            Genre = newGenre;
        }
    }}
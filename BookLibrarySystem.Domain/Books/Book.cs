using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books.Events;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Domain.Books
{
    public sealed class Book : Entity
    {
        private Book(
            Guid id,
            Title title,
            Description description,
            DateTime publicationDate,
            int pages,
            bool isAvailable,
            Guid authorId)
            : base(id)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (publicationDate == default) throw new ArgumentException("Invalid publication date.", nameof(publicationDate));
            if (pages <= 0) throw new ArgumentOutOfRangeException(nameof(pages), "Pages must be greater than zero.");
            if (authorId == Guid.Empty) throw new ArgumentException("Author ID must not be empty.", nameof(authorId));

            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            Pages = pages;
            IsAvailable = isAvailable;
            AuthorId = authorId;
            Genres = new List<BookGenre>();
        }

        private Book() { } // For EF Core

        public Title Title { get; private set; }
        public Description Description { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public int Pages { get; private set; }
        public bool IsAvailable { get; private set; }
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; }
        public ICollection<BookGenre> Genres { get; private set; }

        public static Book Create(Title title, Description description, DateTime publicationDate, int pages, Guid authorId)
        {
            var book = new Book(
                Guid.NewGuid(),
                title,
                description,
                publicationDate,
                pages,
                isAvailable: true,
                authorId: authorId);
                book.RaiseDomainEvent(new BookCreatedDomainEvent(book.Id));
            
            return book;
        }

        public void UpdateDetails(Title title, Description description, DateTime publicationDate, int pages, Guid authorId)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (publicationDate == default) throw new ArgumentException("Invalid publication date.", nameof(publicationDate)); 
            if (pages <= 0) throw new ArgumentOutOfRangeException(nameof(pages), "Pages must be greater than zero."); 
            if (authorId == Guid.Empty) throw new ArgumentException("Author ID must not be empty.", nameof(authorId)); 

            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            Pages = pages;
            AuthorId = authorId;
        }


        public Result MarkAsUnavailable()
        {
            if (!IsAvailable)
            {
                return Result.Failure(BookErrors.BookUnavailable);
            }

            IsAvailable = false;
            return Result.Success();
        }
        public Result MarkAsAvailable()
        {
            if (IsAvailable)
            {
                return Result.Failure(BookErrors.BookAlreadyAvailable);
            }

            IsAvailable = true;
            return Result.Success();
        }


        public Result AddGenre(Genre genre)
        {
            if (genre == null) return Result.Failure(BookErrors.InvalidGenre);

            if (Genres.Any(bg => bg.GenreId == genre.Id))
            {
                return Result.Failure(BookErrors.DuplicateGenre);
            }

            Genres.Add(new BookGenre(Id, genre.Id));
            return Result.Success();
        }
        public void RemoveGenre(Genre genre)
        {
            if (genre == null) throw new ArgumentNullException(nameof(genre));

            var bookGenre = Genres.FirstOrDefault(bg => bg.GenreId == genre.Id);
            if (bookGenre != null)
            {
                Genres.Remove(bookGenre);
            }
        }

    }
}

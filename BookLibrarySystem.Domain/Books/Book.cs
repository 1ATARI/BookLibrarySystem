using BookLibrarySystem.Domain.Abstraction;
using System;
using System.Collections.Generic;
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
            bool isAvailable)
            : base(id)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (publicationDate == default) throw new ArgumentException("Invalid publication date.", nameof(publicationDate));
            if (pages <= 0) throw new ArgumentOutOfRangeException(nameof(pages), "Pages must be greater than zero.");

            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            Pages = pages;
            IsAvailable = isAvailable;
            Genres = new List<BookGenre>();
        }

        private Book() { } // For EF Core

        public Title Title { get; private set; }
        public Description Description { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public int Pages { get; private set; }
        public bool IsAvailable { get; private set; }

        public ICollection<BookGenre> Genres { get; private set; }

        public static Book Create(Title title, Description description, DateTime publicationDate, int pages)
        {
            return new Book(
                Guid.NewGuid(),
                title,
                description,
                publicationDate,
                pages,
                isAvailable: true);
        }

        public void UpdateDetails(Title title, Description description)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));
            if (description == null) throw new ArgumentNullException(nameof(description));

            Title = title;
            Description = description;
        }

        public Result MarkAsUnavailable()
        {
            if (!IsAvailable)
            {
                return Result.Failure(BookErrors.BookAlreadyUnavailable);
            }

            IsAvailable = false;
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
    }
}

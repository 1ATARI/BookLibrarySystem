using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.BooksGenres;
using System;
using System.Collections.Generic;

namespace BookLibrarySystem.Domain.Genres
{
    public sealed class Genre : Entity
    {
        private Genre(Guid id, Name name, Description description) : base(id)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (description == null) throw new ArgumentNullException(nameof(description));

            Name = name;
            Description = description;
            Books = new List<BookGenre>();
        }

        private Genre() { } // For EF Core

        public Name Name { get; private set; }
        public Description Description { get; private set; }

        public ICollection<BookGenre> Books { get; private set; }

        public static Genre Create(Name name, Description description)
        {
            return new Genre(Guid.NewGuid(), name, description);
        }
        public void UpdateDetails(Name name, Description description)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (description == null) throw new ArgumentNullException(nameof(description));

            Name = name;
            Description = description;

        }
    }
    
}
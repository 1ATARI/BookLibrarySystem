using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users.Events;
using Microsoft.AspNetCore.Identity;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Domain.Users
{
    public sealed class ApplicationUser : IdentityUser<Guid>, IIdentifiable
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Name Name { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public ICollection<UserBook> BorrowedBooks { get; private set; } = new List<UserBook>();

        private ApplicationUser(Guid id, Name name, DateTime dateOfBirth)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (dateOfBirth == default) throw new ArgumentException("Invalid date of birth.", nameof(dateOfBirth));

            Id = id;
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        private ApplicationUser() { }

        public static ApplicationUser Create(Name name, DateTime dateOfBirth, string email, string username)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty.", nameof(username));

            var user = new ApplicationUser(Guid.NewGuid(), name, dateOfBirth)
            {
                Email = email,
                UserName = username
            };

            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            return user;
        }

        public void UpdateName(string firstName, string lastName)
        {
            Name = new Name(firstName, lastName);
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public Guid Id { get; set; }
    }
}
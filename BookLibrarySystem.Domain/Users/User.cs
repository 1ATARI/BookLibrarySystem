using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users.Events;
using Microsoft.AspNetCore.Identity;
using System;

namespace BookLibrarySystem.Domain.Users
{
    public sealed class User : IdentityUser<Guid>
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        private User(Guid id, Name name, DateTime dateOfBirth)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (dateOfBirth == default) throw new ArgumentException("Invalid date of birth.", nameof(dateOfBirth));

            Id = id;
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        private User() { }

        public Name Name { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public static User Create(Name name, DateTime dateOfBirth, string email, string username)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username cannot be empty.", nameof(username));

            var user = new User(Guid.NewGuid(), name, dateOfBirth)
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
    }
}
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Books.Events;

public sealed record BookCreatedDomainEvent(Guid BookId) : IDomainEvent;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;
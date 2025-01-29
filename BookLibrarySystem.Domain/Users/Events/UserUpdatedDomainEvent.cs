using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Users.Events;

public sealed record UserUpdatedDomainEvent(Guid UserId) : IDomainEvent;
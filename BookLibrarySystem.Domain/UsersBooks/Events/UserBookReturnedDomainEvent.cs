using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.UsersBooks.Events;

public sealed record UserBookReturnedDomainEvent(Guid Id, Guid UserId, Guid BookId, DateTime ReturnedDate)
    : IDomainEvent;

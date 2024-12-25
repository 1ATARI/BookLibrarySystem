using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.UsersBooks.Events;

public sealed record UserBookDomainEvent(Guid UserId, Guid BookId,DateTime BookingDate) : IDomainEvent;

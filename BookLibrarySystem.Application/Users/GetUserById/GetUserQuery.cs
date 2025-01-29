using BookLibrarySystem.Application.Abstractions.Messaging;

namespace BookLibrarySystem.Application.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;

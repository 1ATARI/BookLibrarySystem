using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Users.GetUser;

namespace BookLibrarySystem.Application.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;

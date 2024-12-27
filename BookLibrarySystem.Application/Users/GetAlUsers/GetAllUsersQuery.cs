using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Users.GetAlUsers;

public sealed record GetAllUsersQuery : IQuery<IEnumerable<User>>;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Users.GetAlUsers;

public sealed record GetAllUsersQuery : IQuery<IEnumerable<ApplicationUser>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
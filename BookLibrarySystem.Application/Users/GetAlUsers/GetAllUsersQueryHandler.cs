using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using MediatR;

namespace BookLibrarySystem.Application.Users.GetAlUsers;

internal sealed class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<ApplicationUser>>
{
    private readonly IApplicationUserRepository _applicationUserRepository;

    public GetAllUsersQueryHandler(IApplicationUserRepository applicationUserRepository)
    {
        _applicationUserRepository = applicationUserRepository;
    }

    public async Task<Result<IEnumerable<ApplicationUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {

        var users = await _applicationUserRepository.GetAllAsync(cancellationToken: cancellationToken);
        
        return Result.Success(users);
    }
}
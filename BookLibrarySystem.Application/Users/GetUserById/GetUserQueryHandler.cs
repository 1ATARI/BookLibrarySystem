using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Users.GetUser;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Users.GetUserById;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        var response = new UserResponse
        {
            Id = user.Id,
            FirstName = user.Name.FirstName,
            LastName = user.Name.LastName,
            Email = user.Email,
            Username = user.UserName
        };

        return response;
    }
}
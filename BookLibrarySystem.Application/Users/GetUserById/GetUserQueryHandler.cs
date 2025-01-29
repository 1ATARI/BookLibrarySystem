using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Users.GetUserById;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IApplicationUserRepository _applicationUserRepository;

    public GetUserQueryHandler(IApplicationUserRepository applicationUserRepository)
    {
        _applicationUserRepository = applicationUserRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _applicationUserRepository.GetByIdAsync(request.UserId,"BorrowedBooks,BorrowedBooks.Book" ,cancellationToken);
        if (user == null)
        {
            return Result.Failure<UserResponse>(ApplicationUserErrors.NotFound);
        }

        var response = new UserResponse
        (
            user.Id,
            user.Name.FirstName,
            user.Name.LastName,
            user.Email!,
            user.UserName!
        );

        return response;
    }
}
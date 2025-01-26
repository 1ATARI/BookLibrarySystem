using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result>
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _applicationUserRepository = applicationUserRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _applicationUserRepository.GetByIdAsync(request.UserId ,null, cancellationToken);
        if (user == null)
        {
            return Result.Failure(ApplicationUserErrors.NotFound);
        }

        if (!string.IsNullOrWhiteSpace(request.FirstName) || !string.IsNullOrWhiteSpace(request.LastName))
        {
            user.UpdateName(request.FirstName ?? user.Name.FirstName, request.LastName ?? user.Name.LastName);
        }
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            user.Email = request.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            user.UserName = request.Username;
        }

        var identityResult = await _userManager.UpdateAsync(user);
        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure(new Error("UserUpdateFailed", errors));
        }

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}
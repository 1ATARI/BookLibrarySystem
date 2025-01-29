using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Application.Users.RegisterUser;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.Users.Events;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IApplicationUserRepository applicationUserRepository,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _applicationUserRepository = applicationUserRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

   public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userDto = request.UserDto;

            var user = await _applicationUserRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);
            if (user == null)
            {
                return Result.Failure(ApplicationUserErrors.NotFound);
            }

            if (!string.IsNullOrWhiteSpace(userDto.Email) && userDto.Email != user.Email)
            {
                var existingUserWithEmail = await _userManager.FindByEmailAsync(userDto.Email);
                if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
                {
                    return Result.Failure(ApplicationUserErrors.EmailExists);
                }
            }

            if (!string.IsNullOrWhiteSpace(userDto.Username) && userDto.Username != user.UserName)
            {
                var existingUserWithUsername = await _userManager.FindByNameAsync(userDto.Username);
                if (existingUserWithUsername != null && existingUserWithUsername.Id != user.Id)
                {
                    return Result.Failure(ApplicationUserErrors.UsernameExists);
                }
            }

            var email = userDto.Email ?? user.Email;

            user.Update(
                userDto.FirstName ?? user.Name.FirstName,
                userDto.LastName ?? user.Name.LastName,
                userDto.Email ?? user.Email,
                userDto.Username ?? user.UserName,
                userDto.PhoneNumber ?? user.PhoneNumber
            );

                var passwordResetResult = await _userManager.RemovePasswordAsync(user);
                if (!passwordResetResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResetResult.Errors.Select(e => e.Description));
                    return Result.Failure(new Error("PasswordUpdateFailed", errors));
                }

                if ( userDto.Password != null)
                {
                    
               
                var addPasswordResult = await _userManager.AddPasswordAsync(user, userDto.Password);
                if (!addPasswordResult.Succeeded)
                {
                    var errors = string.Join(", ", addPasswordResult.Errors.Select(e => e.Description));
                    return Result.Failure(new Error("PasswordUpdateFailed", errors));
                }
                }

            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return Result.Failure(new Error("UserUpdateFailed", errors));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (ConcurrencyException)
        {
            return Result.Failure(ApplicationUserErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("DatabaseError", ex.Message));
        }
    }
}
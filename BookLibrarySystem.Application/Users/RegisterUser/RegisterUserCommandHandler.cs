using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<ApplicationUser>>
{
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IApplicationUserRepository applicationUserRepository, UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _applicationUserRepository = applicationUserRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result<ApplicationUser>>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var name = new Name(request.FirstName, request.LastName);

            var user = ApplicationUser.Create(name, request.DateOfBirth, request.Email, request.Username);

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return Result.Failure<ApplicationUser>(ApplicationUserErrors.NotFound);
            }

            await _applicationUserRepository.AddAsync(user, cancellationToken);


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(user);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<ApplicationUser>(ApplicationUserErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure<ApplicationUser>(new Error("DatabaseError", ex.Message));
        }
    }
}
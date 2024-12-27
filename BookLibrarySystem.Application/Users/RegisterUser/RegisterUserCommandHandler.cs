using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUserRepository userRepository, UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Result<User>>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var name = new Name(request.firstName, request.lastName);

            var user = User.Create(name, request.dateOfBirth, request.email, request.username);

            var identityResult = await _userManager.CreateAsync(user, request.password);
            if (!identityResult.Succeeded)
            {
                var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                return Result.Failure<User>(UserErrors.NotFound);
            }

            await _userRepository.AddAsync(user, cancellationToken);


            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(user);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<User>(UserErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure<User>(new Error("DatabaseError", ex.Message));
        }
    }
}
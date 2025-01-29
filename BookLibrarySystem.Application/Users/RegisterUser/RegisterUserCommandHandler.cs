using BookLibrarySystem.Application.Abstractions.Identity;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;


namespace BookLibrarySystem.Application.Users.RegisterUser;

 internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponseDTO>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUserManager _userManager;

        public RegisterUserCommandHandler(IApplicationUserRepository applicationUserRepository, IUserManager userManager )
        {
            _applicationUserRepository = applicationUserRepository;
            _userManager = userManager;
        }

        public async Task<Result<RegisterUserResponseDTO>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(request.CommandDTO.Username, cancellationToken);
                var existingUserEmail = await _userManager.FindByEmailAsync(request.CommandDTO.Email, cancellationToken);

                if (existingUser != null || existingUserEmail != null)
                {
                    return Result.Failure<RegisterUserResponseDTO>(new Error("UserAlreadyExists", "The username or email is already taken."));
                }
                var name = new Name(request.CommandDTO.FirstName, request.CommandDTO.LastName);
                var user = ApplicationUser.Create(name, request.CommandDTO.DateOfBirth, request.CommandDTO.Email, request.CommandDTO.Username);

                var identityResult = await _userManager.CreateAsync(user, request.CommandDTO.Password, cancellationToken);
                if (!identityResult.Succeeded)
                {
                    var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    return Result.Failure<RegisterUserResponseDTO>(new Error("UserCreationFailed", errors));
                }

                await _applicationUserRepository.AddAsync(user, cancellationToken);

                var userResponse = new RegisterUserResponseDTO(
                    user.Id,
                    user.Name.FirstName,
                    user.Name.LastName,
                    user.Email!,
                    user.UserName!,
                    user.DateOfBirth
                );

                return Result.Success(userResponse);
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<RegisterUserResponseDTO>(ApplicationUserErrors.Overlap);
            }
            catch (Exception ex)
            {
                return Result.Failure<RegisterUserResponseDTO>(new Error("DatabaseError", ex.Message));
            }
        }
    }

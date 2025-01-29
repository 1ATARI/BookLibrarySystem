using BookLibrarySystem.Application.Abstractions.Identity;
using BookLibrarySystem.Application.Abstractions.JWT;
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;


namespace BookLibrarySystem.Application.Users.LoginUser;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    
    private readonly IUserManager _userManager;
    private readonly ISignInManager _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginUserCommandHandler(
        IUserManager userManager,
        ISignInManager signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userByName = await _userManager.FindByNameAsync(request.LoginUserRequestDTO.UserName, cancellationToken);
        var userByEmail = await _userManager.FindByEmailAsync(request.LoginUserRequestDTO.UserName, cancellationToken);
        var user = userByName ?? userByEmail;

        if (user == null)
        {
            return Result.Failure<LoginUserResponse>(new Error("InvalidCredentials", "Invalid Credentials Provided."));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginUserRequestDTO.Password, false , cancellationToken: cancellationToken);
        if (!result.Succeeded)
        {
            return Result.Failure<LoginUserResponse>(new Error("InvalidCredentials", "Invalid username or password."));
        }

        var token = _jwtTokenService.GenerateToken(user);
        var response = new LoginUserResponse(token);

        return Result.Success(response);
    }
}
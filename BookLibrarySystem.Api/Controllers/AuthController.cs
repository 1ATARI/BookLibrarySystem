using BookLibrarySystem.Application.Users.LoginUser;
using BookLibrarySystem.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Api.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommandDTO register, CancellationToken cancellationToken = default)
        {

            var command = new RegisterUserCommand(register);
            var result = await _sender.Send(command,cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value); 
            }

            return BadRequest(result.Error); 
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDTO loginRequest, CancellationToken cancellationToken = default)
        {

            var command = new LoginUserCommand(loginRequest);
            var result = await _sender.Send(command,cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value); 
            }

            return BadRequest(result.Error); 
        }

    }


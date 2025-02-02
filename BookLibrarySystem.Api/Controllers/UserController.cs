using BookLibrarySystem.Application.Users.GetAlUsers;
using BookLibrarySystem.Application.Users.GetUserById;
using BookLibrarySystem.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Api.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet]

        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 0,
            CancellationToken cancellationToken = default)
        {
            if (pageSize == 0)
            {
                pageSize = int.MaxValue;
            }
            var query = new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize };
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserByIdAsync(
            [FromRoute]Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = new GetUserByIdQuery(userId);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUserAsync(
            Guid userId,
            [FromBody] UpdateUserDto userDto,
            CancellationToken cancellationToken = default)
        {
        
            var query = new UpdateUserCommand(userId, userDto);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok() : NotFound(result.Error);
        }
        
    }


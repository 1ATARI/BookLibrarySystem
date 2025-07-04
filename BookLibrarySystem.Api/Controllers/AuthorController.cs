using BookLibrarySystem.Api.Dto;
using BookLibrarySystem.Application.Authors.AddAuthor;
using BookLibrarySystem.Application.Authors.AddBookToAuthor;
using BookLibrarySystem.Application.Authors.DeleteAuthor;
using BookLibrarySystem.Application.Authors.GetAllAuthors;
using BookLibrarySystem.Application.Authors.GetAuthorById;
using BookLibrarySystem.Application.Authors.UpdateAuthor;
using BookLibrarySystem.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorController : ControllerBase
{
    private readonly ISender _sender;

    public AuthorController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAuthorsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
        {
            pageSize = 10;
        }

        var query = new GetAllAuthorQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("{authorId:guid}")]
    public async Task<IActionResult> GetAuthorByIdAsync(
        [FromRoute]Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAuthorByIdQuery(authorId);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuthorAsync(
        [FromBody] AddAuthorCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateAuthorAsync(
        [FromBody] UpdateAuthorCommand request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _sender.Send(request, cancellationToken);


        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{authorId:guid}")]
    public async Task<IActionResult> DeleteAuthorAsync(
        Guid authorId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteAuthorCommand(authorId);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    /// <summary>
    /// Adds a new book to an existing author.
    /// </summary>
    /// <param name="request">The command containing the author ID and book details.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>An IActionResult indicating the result of the operation.</returns>
    [HttpPost("books")]
    public async Task<IActionResult> AddBookToAuthor(
        [FromBody] AddBookToAuthorCommand request,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _sender.Send(request, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
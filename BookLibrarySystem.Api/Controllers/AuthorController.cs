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

    [HttpGet]
    public async Task<IActionResult> GetAllAuthorsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 0,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
        {
            pageSize = int.MaxValue;
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
        [FromBody] AuthorDto authorDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var name = new Name(authorDto.FirstName, authorDto.LastName);
        var command = new AddAuthorCommand(name);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }


    [HttpPut("{authorId:guid}")]
    public async Task<IActionResult> UpdateAuthorAsync(
        Guid authorId,
        [FromBody] AuthorDto authorDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var name = new Name(authorDto.FirstName, authorDto.LastName);
        var command = new UpdateAuthorCommand(authorId, name);
        var result = await _sender.Send(command, cancellationToken);

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
    /// This method to add new book to existing Author 
    /// </summary>
    /// <param name="authorId">Author ID</param>
    /// <param name="addBookDto">Book details </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{authorId:guid}/books")]
    public async Task<IActionResult> AddBookToAuthor(
        Guid authorId,
        [FromBody] AddBookDto addBookDto,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new AddBookToAuthorCommand(authorId, addBookDto);
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
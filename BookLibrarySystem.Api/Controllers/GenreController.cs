using BookLibrarySystem.Api.Dto;
using BookLibrarySystem.Application.Genres.CreateGenre;
using BookLibrarySystem.Application.Genres.DeleteGenre;
using BookLibrarySystem.Application.Genres.GetAllGenres;
using BookLibrarySystem.Application.Genres.GetGenreById;
using BookLibrarySystem.Application.Genres.UpdateGenre;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class GenreController : ControllerBase
{
    private readonly ISender _sender;

    public GenreController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 0,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
        {
            pageSize = int.MaxValue;
        }

        var query = new GetAllGenresQuery { PageNumber = pageNumber, PageSize = pageSize };

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("{genreId:guid}")]
    public async Task<IActionResult> GetGenreById(Guid genreId, CancellationToken cancellationToken = default)
    {
        var query = new GetGenreByIdQuery(GenreId: genreId);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenreAsync(
        GenreDto genreDto,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateGenreCommand(genreDto.Name, genreDto.Description);
        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : NotFound(result.Error);
    }
    
    [HttpPost("{genreId:guid}")]
    public async Task<IActionResult> UpdateGenreAsync(
        Guid genreId,
        [FromBody] GenreDto genreDto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateGenreCommand(genreId , genreDto.Name, genreDto.Description);
        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : NotFound(result.Error);
    }

    [HttpDelete("{genreId:guid}")]
    public async Task<IActionResult> DeleteGenreAsync(
        Guid genreId,
        CancellationToken cancellationToken = default)
    {

        var command = new DeleteGenreCommand(genreId);
        var result = await _sender.Send(command , cancellationToken);
        return result.IsSuccess ? Ok() : NotFound(result.Error);
        
    }
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Genres.Dto;
using BookLibrarySystem.Application.Models;

namespace BookLibrarySystem.Application.Genres.GetAllGenres;

public sealed record GetAllGenresQuery : IQuery<PagedResult<GenreResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
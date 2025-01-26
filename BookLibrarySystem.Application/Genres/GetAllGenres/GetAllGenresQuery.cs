using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Genres.Dto;

namespace BookLibrarySystem.Application.Genres.GetAllGenres;

public sealed record GetAllGenresQuery : IQuery<IEnumerable<GenreResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
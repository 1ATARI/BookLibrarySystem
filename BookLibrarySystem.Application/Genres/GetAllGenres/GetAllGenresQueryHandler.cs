using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Genres.Dto;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetAllGenres;

internal sealed class GetAllGenresQueryHandler : IQueryHandler<GetAllGenresQuery, IEnumerable<GenreResponseDto>>
{
    private readonly IGenreRepository _genreRepository;

    public GetAllGenresQueryHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Result<IEnumerable<GenreResponseDto>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        int skip =(request.PageNumber - 1) * request.PageSize; 
        var genres = await _genreRepository.GetAllAsync(
            skip: skip,
            take: request.PageSize,
            cancellationToken: cancellationToken);

        var genreResponse = genres.Select(g => new GenreResponseDto(g.Id, g.Name.Value, g.Description.Value));

        return Result.Success(genreResponse);
    }
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Genres.Dto;
using BookLibrarySystem.Application.Models;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetAllGenres;

internal sealed class GetAllGenresQueryHandler : IQueryHandler<GetAllGenresQuery, PagedResult<GenreResponseDto>>
{
    private readonly IGenreRepository _genreRepository;

    public GetAllGenresQueryHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Result<PagedResult<GenreResponseDto>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;
        
        var totalCount = await _genreRepository.GetCountAsync(cancellationToken: cancellationToken);
        var genres = await _genreRepository.GetAllAsync(
            skip: skip,
            take: request.PageSize,
            cancellationToken: cancellationToken);

        var genreResponses = genres
            .Select(g => new GenreResponseDto(g.Id, g.Name.Value, g.Description.Value))
            .ToList();

        var pagedResult = new PagedResult<GenreResponseDto>(
            genreResponses,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result.Success(pagedResult);
    }
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetAllGenres;

internal sealed class GetAllGenresQueryHandler : IQueryHandler<GetAllGenresQuery, IEnumerable<GenreResponse>>
{
    private readonly IGenreRepository _genreRepository;

    public GetAllGenresQueryHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<Result<IEnumerable<GenreResponse>>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = await _genreRepository.GetAllAsync(cancellationToken);

        var genreResponse = genres.Select(g => new GenreResponse(g.Id, g.Name.Value, g.Description.Value));

        return Result.Success(genreResponse);
    }
}
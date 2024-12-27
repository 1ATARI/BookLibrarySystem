using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetGenreById;

public class GetGenreByIdQueryHandler
{
    
    private readonly IGenreRepository _genreRepository;

    public GetGenreByIdQueryHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }


    public async Task<Result<Genre>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(request.GenreId , cancellationToken);

        if (genre == null)
        {
            return Result.Failure<Genre>(GenreErrors.NotFound);
        }

        return genre;
    }
}

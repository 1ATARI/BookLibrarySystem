using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Genres.Dto;
using BookLibrarySystem.Application.Genres.GetAllGenres;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetGenreById;

public class GetGenreByIdQueryHandler : IQueryHandler<GetGenreByIdQuery, GenreResponseDto>
{
    
    private readonly IGenreRepository _genreRepository;

    public GetGenreByIdQueryHandler(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }


    public async Task<Result<GenreResponseDto>> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.GetByIdAsync(request.GenreId,cancellationToken: cancellationToken);

        if (genre == null)
        {
            return Result.Failure<GenreResponseDto>(GenreErrors.NotFound);
        }
        var genreResponse = new GenreResponseDto(genre.Id, genre.Name.Value, genre.Description.Value);


        return Result.Success<GenreResponseDto>(genreResponse);
    }
}

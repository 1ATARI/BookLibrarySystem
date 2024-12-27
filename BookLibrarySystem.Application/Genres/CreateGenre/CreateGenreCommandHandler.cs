using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.CreateGenre;

internal sealed class CreateGenreCommandHandler : ICommandHandler<CreateGenreCommand, Genre>
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Genre>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result.Failure<Genre>(GenreErrors.InvalidName);
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return Result.Failure<Genre>(GenreErrors.InvalidDescription);
            }

            var exists = await _genreRepository.ExistsByNameAsync(new Name(request.Name) ,cancellationToken);
            if (exists)
            {
                return Result.Failure<Genre>(GenreErrors.DuplicateGenre);
            }

            // Create a new genre
            var genre = Genre.Create(new Name(request.Name), new Description(request.Description));

            await _genreRepository.AddAsync(genre,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return genre;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Genre>(GenreErrors.Overlap);
        }
        catch (Exception ex)
        {
            return Result.Failure<Genre>(new Error("DatabaseError", ex.Message));
        }
    }
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;
using BookLibrarySystem.Domain.Users;
using Name = BookLibrarySystem.Domain.Genres.Name;

namespace BookLibrarySystem.Application.Genres.UpdateGenre;

internal sealed class UpdateGenreCommandHandler : ICommandHandler<UpdateGenreCommand, Genre>
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Genre>> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var genre = await _genreRepository.GetByIdAsync(request.Id, cancellationToken);
            if (genre == null)
            {
                return Result.Failure<Genre>(GenreErrors.NotFound);
            }


            genre.UpdateDetails(new Name(request.Name), new Description(request.Description));

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
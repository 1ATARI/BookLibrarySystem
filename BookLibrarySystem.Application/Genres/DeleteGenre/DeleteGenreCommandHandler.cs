using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.DeleteGenre;

internal sealed class DeleteGenreCommandHandler : ICommandHandler<DeleteGenreCommand>
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exists = await _genreRepository.ExistsByIdAsync(request.Id ,cancellationToken);
            if (!exists)
            {
                return Result.Failure(GenreErrors.NotFound);
            }

            await _genreRepository.DeleteAsync(request.Id ,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
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
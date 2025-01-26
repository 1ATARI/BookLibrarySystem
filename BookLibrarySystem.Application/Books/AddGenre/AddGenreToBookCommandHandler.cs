using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Books.AddGenre
{
    public class AddGenreToBookCommandHandler : ICommandHandler<AddGenreToBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddGenreToBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork,
            IGenreRepository genreRepository)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
            _genreRepository = genreRepository;
        }

        public async Task<Result> Handle(AddGenreToBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(request.BookId, "Genres", cancellationToken);
                if (book == null)
                {
                    return Result.Failure(BookErrors.NotFound);
                }

                // Validate genre existence
                var genre = await _genreRepository.GetByIdAsync(request.GenreId ,null, cancellationToken);
                if (genre == null)
                {
                    return Result.Failure(GenreErrors.NotFound);
                }

                // Check if the genre is already associated with the book
                var result = book.AddGenre(genre);
                if (result.IsFailure)
                {
                    return Result.Failure(GenreErrors.DuplicateGenre);
                }

                // Save changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (ConcurrencyException)
            {
                return Result.Failure(BookErrors.Overlap);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("DatabaseError", ex.Message));
            }
        }
    }
}
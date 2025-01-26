using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Exceptions;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;


namespace BookLibrarySystem.Application.BooksGenres.UpdateBookGenre
{
    public class UpdateGenreForBookCommandHandler : ICommandHandler<UpdateGenreForBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGenreForBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork,
            IGenreRepository genreRepository)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
            _genreRepository = genreRepository;
        }

        public async Task<Result> Handle(UpdateGenreForBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate book existence
                var book = await _bookRepository.GetByIdAsync(request.BookId ,"Book,Genre",cancellationToken);
                if (book == null)
                {
                    return Result.Failure(BookErrors.BookNotFound); // Book not found error
                }

                var oldGenre = await _genreRepository.GetByIdAsync(request.OldGenreId ,null,cancellationToken);
                if (oldGenre == null)
                {
                    return Result.Failure(BookErrors.GenreNotFound);
                }

                // Validate new genre existence
                var newGenre = await _genreRepository.GetByIdAsync(request.NewGenreId ,null, cancellationToken);
                if (newGenre == null)
                {
                    return Result.Failure(BookErrors.NewGenreNotFound);
                }

                var bookGenre = book.Genres.FirstOrDefault(bg => bg.GenreId == oldGenre.Id);
                if (bookGenre == null)
                {
                    return Result.Failure(BookErrors.GenreNotAssociated);
                }

                bookGenre.UpdateGenre(newGenre);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<Genre>(BookGenreErrors.Overlap);
            }
            catch (Exception ex)
            {
                return Result.Failure<Genre>(new Error("DatabaseError", ex.Message));
            }
        }
    }
}
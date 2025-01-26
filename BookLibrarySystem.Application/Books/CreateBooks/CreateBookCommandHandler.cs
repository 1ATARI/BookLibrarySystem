using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Genres;
using Description = BookLibrarySystem.Domain.Books.Description;

namespace BookLibrarySystem.Application.Books.CreateBooks;
public class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Book>
{
    private readonly IBookRepository _bookRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookCommandHandler(
        IBookRepository bookRepository,
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Book>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var bookDto = request.BookDto;

        var title = new Title(bookDto.Title);
        var description = new Description(bookDto.Description);

        var book = Book.Create(title, description, bookDto.PublicationDate, bookDto.Pages, bookDto.AuthorId);

        book.MarkAsAvailable(); 

        foreach (var genreId in bookDto.GenreIds)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId,cancellationToken: cancellationToken);
            if (genre != null)
            {
                var result = book.AddGenre(genre);
                if (result.IsFailure)
                {
                    return Result.Failure<Book>(result.Error);
                }
            }
        }

        await _bookRepository.AddAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return book;
    }
}
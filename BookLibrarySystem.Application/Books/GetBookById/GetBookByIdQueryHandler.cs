using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Books.Dto;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
namespace BookLibrarySystem.Application.Books.GetBookById;

public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, BookResponseDto>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<BookResponseDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, "Author,Genres.Genre", cancellationToken);

        if (book == null)
        {
            return Result.Failure<BookResponseDto>(BookErrors.NotFound);
        }

        var bookDto = new BookResponseDto(
            book.Id, 
            book.Title.Value, 
            book.Description.Value, 
            book.PublicationDate, 
            book.Pages, 
            book.IsAvailable,
            book.Genres.Select(bg => new GenreDto(
                bg.Genre.Id, 
                bg.Genre.Name.Value
            )).ToList(),
            new AuthorDto(
                book.Author.Id,
                book.Author.Name.FirstName,
                book.Author.Name.LastName
            )
        );
       

        return Result.Success(bookDto);
    }
}

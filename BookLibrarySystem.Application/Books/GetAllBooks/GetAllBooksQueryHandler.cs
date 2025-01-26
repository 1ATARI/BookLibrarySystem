using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Books.Dto;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.GetAllBooks;

public class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, IEnumerable<BookResponseDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<IEnumerable<BookResponseDto>>> Handle(GetAllBooksQuery request,
        CancellationToken cancellationToken)
    {
        int skip = (request.PageNumber - 1) * request.PageSize;


        var books = await _bookRepository.GetAllAsync(
            skip: skip,
            take: request.PageSize,
            includeProperties: "Author,Genres.Genre", 
            cancellationToken: cancellationToken);

        var bookDto = books.Select(b => new BookResponseDto(
            b.Id,
            b.Title.Value,
            b.Description.Value,
            b.PublicationDate,
            b.Pages,
            b.IsAvailable,
            b.Genres.Select(
                bg => new GenreDto(
                    bg.Genre.Id,
                    bg.Genre.Name.Value
                    )).ToList(),
            new AuthorDto(
                b.Author.Id,
                b.Author.Name.FirstName,
                b.Author.Name.LastName
            )
        ));

        return Result.Success(bookDto);
    }
}
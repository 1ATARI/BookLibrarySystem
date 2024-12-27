using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.BooksGenres;

namespace BookLibrarySystem.Application.BooksGenres.GetBookGenre;

public class GetBookGenreQueryHandler : IQueryHandler<GetBookGenreQuery, Result<BookGenre>>
{
    private readonly IBookGenreRepository _bookGenreRepository;

   
    public GetBookGenreQueryHandler(IBookGenreRepository bookGenreRepository)
    {
        _bookGenreRepository = bookGenreRepository;
    }

    public async Task<Result<Result<BookGenre>>> Handle(GetBookGenreQuery request, CancellationToken cancellationToken)
    {
        // Fetch the BookGenre by Id
        var bookGenre = await _bookGenreRepository.GetByIdAsync(request.BookGenreId ,cancellationToken);
        if (bookGenre == null)
        {
            // Return a failure wrapped inside another Result
            return Result.Failure<Result<BookGenre>>(BookGenreErrors.NotFound);
        }

        // Return a success wrapped inside another Result
        return Result.Success(Result.Success(bookGenre));
    }
}
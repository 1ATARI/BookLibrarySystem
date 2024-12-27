using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.GetBookById;

public class GetBookByIdQueryHandler : IQueryHandler<GetBookByIdQuery, Book>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Result<Book>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId ,cancellationToken);

        if (book == null)
        {
            return Result.Failure<Book>(BookErrors.NotFound);
        }

        return book;
    }
}

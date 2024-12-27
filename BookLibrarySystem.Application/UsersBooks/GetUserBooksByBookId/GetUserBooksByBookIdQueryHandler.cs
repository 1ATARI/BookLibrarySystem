using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByBookId;

internal sealed class GetUserBooksByBookIdQueryHandler : IQueryHandler<GetUserBooksByBookIdQuery, IEnumerable<UserBook>>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetUserBooksByBookIdQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<IEnumerable<UserBook>>> Handle(GetUserBooksByBookIdQuery request, CancellationToken cancellationToken)
    {
        var userBooks = await _userBookRepository.GetByBookIdAsync(request.BookId,cancellationToken);
        return Result.Success(userBooks);
    }
}
using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBookById;

internal sealed class GetUserBookByIdQueryHandler : IQueryHandler<GetUserBookByIdQuery, UserBook>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetUserBookByIdQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<UserBook>> Handle(GetUserBookByIdQuery request, CancellationToken cancellationToken)
    {
        var userBook = await _userBookRepository.GetByIdAsync(request.UserBookId ,"Book,User", cancellationToken);
        return userBook == null
            ? Result.Failure<UserBook>(UserBookErrors.UserBookNotFound)
            : userBook;
    }
}
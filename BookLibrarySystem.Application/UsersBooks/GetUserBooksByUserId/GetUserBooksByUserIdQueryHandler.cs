using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByUserId;

internal sealed class GetUserBooksByUserIdQueryHandler : IQueryHandler<GetUserBooksByUserIdQuery, IEnumerable<UserBook>>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetUserBooksByUserIdQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<IEnumerable<UserBook>>> Handle(GetUserBooksByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userBooks = await _userBookRepository.GetByUserIdAsync(request.UserId,cancellationToken);
        return Result.Success(userBooks);
    }
}
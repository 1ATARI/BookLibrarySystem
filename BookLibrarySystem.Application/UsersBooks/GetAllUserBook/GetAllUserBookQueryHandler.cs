using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetAllUserBook;

internal sealed class GetAllUserBookQueryHandler : IQueryHandler<GetAllUserBookQuery, IEnumerable<UserBook>>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetAllUserBookQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async  Task<Result<IEnumerable<UserBook>>> Handle(GetAllUserBookQuery request, CancellationToken cancellationToken)
    {
        var userBooks =await  _userBookRepository.GetAllAsync(cancellationToken);
        
        return Result.Success(userBooks);
    }
}
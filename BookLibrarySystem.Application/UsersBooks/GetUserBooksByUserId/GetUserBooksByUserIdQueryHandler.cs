using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByUserId;

internal sealed class GetUserBooksByUserIdQueryHandler : IQueryHandler<GetUserBooksByUserIdQuery, IEnumerable<UserBookByUserIdDto>>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetUserBooksByUserIdQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<IEnumerable<UserBookByUserIdDto>>> Handle(GetUserBooksByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userBooks = await _userBookRepository.GetByUserIdAsync(request.UserId,cancellationToken);
        if (!userBooks.Any()) 
        {
            return Result.Failure<IEnumerable<UserBookByUserIdDto>>(UserBookErrors.UserBookNotFound);
        }
        var userBookDto = userBooks.Select(ub => new UserBookByUserIdDto(
            ub.Id,
            ub.ApplicationUser.Id,
            ub.ApplicationUser.Name.FirstName,
            ub.ApplicationUser.Name.LastName,
            ub.ApplicationUser.UserName,
            ub.ApplicationUser.Email,
            ub.Book.Id,
            ub.Book.Title.Value,
            ub.ReturnedDate
        )).ToList();
        return userBookDto;    }
}
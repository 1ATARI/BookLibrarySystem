using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByBookId;

internal sealed class GetUserBooksByBookIdQueryHandler : IQueryHandler<GetUserBooksByBookIdQuery, IEnumerable<UserBookByBookIdDto>>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetUserBooksByBookIdQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<IEnumerable<UserBookByBookIdDto>>> Handle(GetUserBooksByBookIdQuery request, CancellationToken cancellationToken)
    {
        var userBooks = await _userBookRepository.GetByBookIdAsync(request.BookId,cancellationToken);
        if (!userBooks.Any()) 
        {
            return Result.Failure<IEnumerable<UserBookByBookIdDto>>(UserBookErrors.UserBookNotFound);
        }
        var userBookDto = userBooks.Select(ub => new UserBookByBookIdDto(
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
        return userBookDto;
    }
}
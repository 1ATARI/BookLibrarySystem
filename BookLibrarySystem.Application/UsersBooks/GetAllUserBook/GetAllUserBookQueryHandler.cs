using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetAllUserBook;

internal sealed class GetAllUserBookQueryHandler : IQueryHandler<GetAllUserBookQuery, IEnumerable<UserBookDto>>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetAllUserBookQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<IEnumerable<UserBookDto>>> Handle(GetAllUserBookQuery request, CancellationToken cancellationToken)

    {
        int skip =(request.PageNumber - 1) * request.PageSize; 

        var userBooks =
            await _userBookRepository.GetAllAsync(null, "ApplicationUser,Book",
                skip: skip,
                take: request.PageSize,
                cancellationToken: cancellationToken);
        var userBookDto = userBooks.Select(ub => new UserBookDto(
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
        return Result.Success<IEnumerable<UserBookDto>>(userBookDto);
    }
}
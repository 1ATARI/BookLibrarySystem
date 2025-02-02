using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.GetUserBookById;

internal sealed class GetUserBookByIdQueryHandler : IQueryHandler<GetUserBookByIdQuery, UserBookByIdDto>
{
    private readonly IUserBookRepository _userBookRepository;

    public GetUserBookByIdQueryHandler(IUserBookRepository userBookRepository)
    {
        _userBookRepository = userBookRepository;
    }

    public async Task<Result<UserBookByIdDto>> Handle(GetUserBookByIdQuery request, CancellationToken cancellationToken)
    {
        var userBook = await _userBookRepository.GetByIdAsync(request.UserBookId ,"ApplicationUser,Book", cancellationToken);
        if (userBook == null)
        {
            return Result.Failure<UserBookByIdDto>(UserBookErrors.UserBookNotFound);
        }
        var userBookDto = new UserBookByIdDto(
            userBook.Id,  
            userBook.ApplicationUser.Id, 
            userBook.ApplicationUser.Name.FirstName,  
            userBook.ApplicationUser.Name.LastName,
            userBook.ApplicationUser.UserName,  
            userBook.ApplicationUser.Email, 
            userBook.Book.Id,  
            userBook.Book.Title.Value  
        );
        
        return Result.Success(userBookDto);

    }
}
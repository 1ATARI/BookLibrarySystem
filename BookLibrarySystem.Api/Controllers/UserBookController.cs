using BookLibrarySystem.Application.Books.DeleteBook;
using BookLibrarySystem.Application.UsersBooks.BorrowUserBook;
using BookLibrarySystem.Application.UsersBooks.DeleteUserBook;
using BookLibrarySystem.Application.UsersBooks.GetAllUserBook;
using BookLibrarySystem.Application.UsersBooks.GetUserBookById;
using BookLibrarySystem.Application.UsersBooks.GetUserBooksByBookId;
using BookLibrarySystem.Application.UsersBooks.GetUserBooksByUserId;
using BookLibrarySystem.Application.UsersBooks.ReturnUserBook;
using BookLibrarySystem.Application.UsersBooks.UpdateUserBook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserBookController : ControllerBase
{
    private readonly ISender _sender;

    public UserBookController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserBooks(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 0,
        CancellationToken cancellationToken = default)
    {
        if (pageSize == 0)
        {
            pageSize = int.MaxValue;
        }

        {
            var query = new GetAllUserBookQuery{ PageNumber = pageNumber, PageSize = pageSize };
            var result = await _sender.Send(query ,cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }

    [HttpGet("{userBookId:guid}")]
    public async Task<IActionResult> GetUserBookById(Guid userBookId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserBookByIdQuery(userBookId);
        var result = await _sender.Send(query,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("book/{bookId:guid}")]
    public async Task<IActionResult> GetUserBooksByBookId(Guid bookId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserBooksByBookIdQuery(bookId);
        var result = await _sender.Send(query,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetUserBooksByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserBooksByUserIdQuery(userId);
        var result = await _sender.Send(query,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }


    [HttpPost("borrow")]
    public async Task<IActionResult> BorrowBook([FromBody]BorrowBookRequest request , CancellationToken cancellationToken = default)
    {
        var command = new BorrowBookCommand(request);
        var result = await _sender.Send(command,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPut("borrow/{userBookId:guid}")]
    public async Task<IActionResult> UpdateUserBook(Guid userBookId ,[FromBody]UpdateUserBookRequest request, CancellationToken cancellationToken = default )
    {
        var command = new UpdateUserBookCommand(userBookId,request);
        var result = await _sender.Send(command,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPut("return/{userBookId:guid}")]
    public async Task<IActionResult> ReturnBook(Guid userBookId ,[FromBody]DateTime returnedDate , CancellationToken cancellationToken = default)
    {
        var command = new ReturnBookCommand(userBookId, returnedDate);
            
        var result = await _sender.Send(command,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        
    }

    [HttpDelete("{userBookId:guid}")]
    public async Task<IActionResult> DeleteUserBook(Guid userBookId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserBookCommand(userBookId);
              
        var result = await _sender.Send(command,cancellationToken);
        return result.IsSuccess ?
            Ok() :
            BadRequest(result.Error);
    }
    
    
    
    
    
    
    
    
    
    
}

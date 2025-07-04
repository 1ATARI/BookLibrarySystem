using BookLibrarySystem.Application.Books.CreateBooks;
using BookLibrarySystem.Application.Books.DeleteBook;
using BookLibrarySystem.Application.Books.GetAllBooks;
using BookLibrarySystem.Application.Books.GetBookById;
using BookLibrarySystem.Application.Books.UpdateBook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrarySystem.Api.Controllers;
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly ISender _sender;

        public BookController(ISender sender)
        {
            _sender = sender;
        }
        
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllBooksAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 0,
            CancellationToken cancellationToken = default)
        {
            if (pageSize == 0)  
            {
                pageSize = 10;
            }
            var query = new GetAllBooksQuery{ PageNumber = pageNumber, PageSize = pageSize };

            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }


        [HttpGet("{bookId:guid}")]
        public async Task<IActionResult> GetBookByIdAsync(
            Guid bookId,
            CancellationToken cancellationToken = default)
        {
            
            var query = new GetBookByIdQuery(bookId);
            var result = await _sender.Send(query, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBookAsync(
            [FromBody] CreateBookCommand request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _sender.Send(request, cancellationToken);
            return result.IsSuccess ? Ok() : NotFound(result.Error);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBookAsync(
            [FromBody] UpdateBookCommand request,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
            var result = await _sender.Send(request, cancellationToken);
        
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBookAsync(DeleteBookCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var result = await _sender.Send(request);
            return result.IsSuccess ? Ok() : NotFound(result.Error);

        }
        
    }

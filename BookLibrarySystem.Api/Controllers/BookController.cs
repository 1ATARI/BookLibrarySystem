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
        
        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 0,
            CancellationToken cancellationToken = default)
        {
            if (pageSize == 0)  
            {
                pageSize = int.MaxValue;
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
            [FromBody] CreateBookDto bookDto,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = new CreateBookCommand(bookDto);
            var result = await _sender.Send(query, cancellationToken);
            return result.IsSuccess ? Ok() : NotFound(result.Error);
        }

        [HttpPut("{bookId:guid}")]
        public async Task<IActionResult> UpdateBookAsync(
            Guid bookId,
            [FromBody] UpdateBookDto bookDto,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { Errors = errors });
                
            }
        
            var command = new UpdateBookCommand(bookId, bookDto);
            var result = await _sender.Send(command, cancellationToken);
        
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBookAsync(Guid bookId)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { Errors = errors });
                
            }

            var command = new DeleteBookCommand(bookId);
            
            var result = await _sender.Send(command);
            return result.IsSuccess ? Ok() : NotFound(result.Error);

        }
        
    }

using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Books.Dto;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.GetAllBooks;

public record GetAllBooksQuery : IQuery<IEnumerable<BookResponseDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
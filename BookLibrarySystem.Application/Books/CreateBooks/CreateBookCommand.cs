using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.CreateBooks;

public record CreateBookCommand (

     string Title ,
     string Description ,
     DateTime PublicationDate,
     int Pages ,
     Guid AuthorId 
): ICommand<Book>;



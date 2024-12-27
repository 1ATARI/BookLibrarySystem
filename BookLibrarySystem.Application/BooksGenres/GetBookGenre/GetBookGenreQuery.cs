using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.BooksGenres;

namespace BookLibrarySystem.Application.BooksGenres.GetBookGenre;

public sealed record GetBookGenreQuery(Guid BookGenreId) : IQuery<Result<BookGenre>>;

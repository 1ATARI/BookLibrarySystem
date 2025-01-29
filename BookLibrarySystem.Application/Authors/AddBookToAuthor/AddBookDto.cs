namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;

public class AddBookDto
{
    
    public required string Title { get; init; }
    public required string Description { get; init; }
    public DateTime PublicationDate { get; init; }
    public int Pages { get; init; }
}

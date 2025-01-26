namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;

public class AddBookDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime PublicationDate { get; init; }
    public int Pages { get; init; }
}

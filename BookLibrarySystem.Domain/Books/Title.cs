namespace BookLibrarySystem.Domain.Books;

public record Title
{
    public string Value { get; }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty or whitespace.", nameof(value));

        Value = value;
    }
}
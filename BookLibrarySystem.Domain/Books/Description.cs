namespace BookLibrarySystem.Domain.Books;

public record Description
{
    public string Value { get; }

    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Description cannot be empty or whitespace.", nameof(value));

        Value = value;
    }
}
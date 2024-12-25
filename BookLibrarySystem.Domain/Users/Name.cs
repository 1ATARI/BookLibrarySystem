namespace BookLibrarySystem.Domain.Users;

public record Name
{
    public string FirstName { get; }
    public string LastName { get; }    
    public Name(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }
}

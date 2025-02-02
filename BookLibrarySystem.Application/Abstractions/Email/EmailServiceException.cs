namespace BookLibrarySystem.Application.Abstractions.Email;

public class EmailServiceException : Exception
{
    public EmailServiceException()
    {
    }

    public EmailServiceException(string message) : base(message)
    {
    }

    public EmailServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

namespace BookLibrarySystem.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(string recipientEmail, string subject, string message);
}
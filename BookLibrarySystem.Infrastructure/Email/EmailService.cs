using BookLibrarySystem.Application.Abstractions.Email;

namespace BookLibrarySystem.Infrastructure.Email;

public class EmailService : IEmailService
{
    public async Task SendAsync(string recipientEmail, string subject, string message)
    {
        throw new NotImplementedException();
    }
}
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.Abstractions.Email;

public interface IEmailService
{
    Task<Result> SendAsync(string recipientEmail, string subject, string message);
}
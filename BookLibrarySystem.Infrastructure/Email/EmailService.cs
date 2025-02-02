using BookLibrarySystem.Application.Abstractions.Email;
using BookLibrarySystem.Domain.Abstraction;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.Extensions.Logging;

namespace BookLibrarySystem.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly string _email;
    private readonly string _displayName;
    private readonly string _password;
    private readonly string _host;
    private readonly int _port;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
    {
        if (mailSettings.Value == null)
        {
            throw new ArgumentNullException(nameof(mailSettings));
        }

        _email = mailSettings.Value.Email ?? throw new ArgumentNullException(nameof(mailSettings.Value.Email));
        _displayName = mailSettings.Value.DisplayName ?? throw new ArgumentNullException(nameof(mailSettings.Value.DisplayName));
        _password = mailSettings.Value.Password ?? throw new ArgumentNullException(nameof(mailSettings.Value.Password));
        _host = mailSettings.Value.Host ?? throw new ArgumentNullException(nameof(mailSettings.Value.Host));
        _port = mailSettings.Value.Port;

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result> SendAsync(string recipientEmail, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
        {
            Result.Failure(new Error("Invalid.Email","Recipient email cannot be null or empty."));
            // throw new ArgumentException("Recipient email cannot be null or empty.", nameof(recipientEmail));
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            Result.Failure(new Error("Invalid.Subject","Subject cannot be null or empty."));

            // throw new ArgumentException("Subject cannot be null or empty.", nameof(subject));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        }

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_displayName, _email));
        emailMessage.To.Add(new MailboxAddress("", recipientEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        try
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_host, _port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_email, _password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

            _logger.LogInformation("Email sent successfully to {RecipientEmail}.", recipientEmail);
            return Result.Success();
        }
        catch (SmtpCommandException ex)
        {
            _logger.LogError(ex, "Failed to send email to {RecipientEmail} due to an SMTP error.", recipientEmail);
            return Result.Failure(new Error("Smtp.Error", "Failed to send email due to an SMTP error."));
        }
        catch (SmtpProtocolException ex)
        {
            _logger.LogError(ex, "Failed to send email to {RecipientEmail} due to a protocol error.", recipientEmail);
            return Result.Failure(new Error("Protocol.Error", "Failed to send email due to a protocol error."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {RecipientEmail}.", recipientEmail);
            return Result.Failure(new Error("Email.Error", "Failed to send email."));
        }
    }
}
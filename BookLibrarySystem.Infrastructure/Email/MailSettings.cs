namespace BookLibrarySystem.Infrastructure.Email;

public class MailSettings
{
    public required string Email { get; set; }
    public required string DisplayName { get; set; }
    public required string Password { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
}
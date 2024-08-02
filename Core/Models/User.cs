namespace ManeKani.Core.Models;

public class User
{
    public required Guid Id { get; set; }
    public required string Email { get; set; }
    public string? Username { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsVerified { get; set; }
    public bool IsComplete { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
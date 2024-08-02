namespace ManeKani.Core.Models;

public class ApiKey : PublicApiKey
{
    public required string Hash { get; set; }
}

public class PublicApiKey
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Prefix { get; set; }
    public ApiKeyClaims Claims { get; set; }

    public DateTimeOffset? UsedAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
}

public struct ApiKeyClaims
{
    public bool DeckWrite;
    public bool DeckDelete;
    public bool SubjectWrite;
    public bool SubjectDelete;
    public bool ReviewCreate;
    public bool StudyDataWrite;
    public bool StudyDataDelete;
}
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
    public bool DeckWrite { get; set; }
    public bool DeckDelete { get; set; }
    public bool SubjectWrite { get; set; }
    public bool SubjectDelete { get; set; }
    public bool ReviewCreate { get; set; }
    public bool StudyDataWrite { get; set; }
    public bool StudyDataDelete { get; set; }
}
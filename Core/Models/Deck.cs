namespace ManeKani.Core.Models;

public class DeckBasic
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public required string Slug { get; set; }
    public string[]? Tags { get; set; }

    public bool IsFeatured { get; set; }
    public bool IsPublic { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
}

public class Deck : DeckBasic
{
    public Guid[]? SubjectIds { get; set; }
    public Guid[]? SubscribedByUserId { get; set; }
}

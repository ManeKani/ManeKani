namespace ManeKani.Core.Adapters;

using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;

public static class DecksAdapter
{
    public class CreateDeckRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public required string Slug { get; set; }
        public string[]? Tags { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsPublic { get; set; }
    }

    public static async Task<Deck> CreateDeck(IDeckRepository repo, Guid userId, CreateDeckRequest r)
    {
        var createdDeck = await repo.CreateDeck(userId, new Deck
        {
            Name = r.Name,
            Description = r.Description,
            Image = r.Image,
            Slug = r.Slug,
            Tags = r.Tags,
            IsFeatured = r.IsFeatured,
            IsPublic = r.IsPublic
        });

        return createdDeck;
    }

    public static async Task<Deck?> GetDeck(IDeckRepository repo, Guid userId, Guid deckId)
    {
        var deck = await repo.GetDeck(deckId);


        // TODO: users allowed by the deck's owner should also be able to view the deck
        if (deck == null || !deck.IsPublic && deck.CreatedByUserId != userId)
        {
            return null;
        }

        return deck;
    }

    public static Task<IEnumerable<DeckBasic>> GetDecks(IDeckRepository repo)
    {
        return repo.GetDecks(Guid.Empty);
    }

    public static Task<Deck> UpdateDeck()
    {
        throw new NotImplementedException();
    }

    public static Task DeleteDeck()
    {
        throw new NotImplementedException();
    }
}

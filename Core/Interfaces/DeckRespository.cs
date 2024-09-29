namespace ManeKani.Core.Interfaces;

using ManeKani.Core.Models;

public interface IDeckRepository
{
    public Task<Deck> CreateDeck(Guid userId, Deck request);
    public Task<Deck?> GetDeck(Guid deckId);
    public Task<IEnumerable<DeckBasic>> GetDecks(Guid userId);
    public Task<Deck> UpdateDeck(Guid deckId, Deck request);
    public Task DeleteDeck(Guid deckId);

}
namespace ManeKani.DB;

using Dapper;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using SqlKata.Execution;

public class DeckRepository : Repository, IDeckRepository
{
    public Task<Deck> CreateDeck(Guid userId, Deck request)
    {
        return Database.Connection.QuerySingleAsync<Deck>(@"
            INSERT INTO decks (name, description, image, slug, tags, is_featured, is_public, created_by_user_id)
                VALUES (@name, @description, @image, @slug, @tags, @is_featured, @is_public, @created_by_user_id)
            RETURNING id, name, description, image, slug, tags, is_featured, is_public, created_at, updated_at, created_by_user_id", new
        {
            name = request.Name,
            description = request.Description,
            image = request.Image,
            slug = request.Slug,
            tags = request.Tags,
            is_featured = request.IsFeatured,
            is_public = request.IsPublic,
            created_by_user_id = userId,
        });
    }

    public Task DeleteDeck(Guid deckId)
    {
        throw new NotImplementedException();
    }

    public Task<Deck?> GetDeck(Guid deckId)
    {
        return Database.Query("decks")
            .Where("id", deckId)
            .Select([
                "id", "name", "description", "image", "slug", "tags",
                "is_featured", "is_public", "created_at", "updated_at",
                "created_by_user_id"
            ])
            .FirstOrDefaultAsync<Deck?>();
    }

    public Task<IEnumerable<DeckBasic>> GetDecks(Guid userId)
    {
        return Database.Query("decks")
            .Select([
                "id", "name", "description", "image", "slug", "tags",
                "is_featured", "is_public", "created_at", "updated_at",
                "created_by_user_id"
            ])
            .GetAsync<DeckBasic>();
    }

    public Task<Deck> UpdateDeck(Guid deckId, Deck request)
    {
        throw new NotImplementedException();
    }
}
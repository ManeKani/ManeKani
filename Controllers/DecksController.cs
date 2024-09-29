namespace ManeKani.Controllers;

using ManeKani.Auth.ApiKey;
using ManeKani.Core.Adapters;
using ManeKani.Core.Models;
using ManeKani.DB;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/deck")]
[ApiController]
public class DecksController : ControllerBase
{
    private readonly ILogger<DecksController> _logger;
    private readonly ManeKaniDatabase _db;

    public DecksController(ILogger<DecksController> logger, ManeKaniDatabase db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet("")]
    [ApiKey]
    // List all decks
    public async Task<IActionResult> GetDecks()
    {
        var decks = await DecksAdapter.GetDecks(_db);

        return Ok(
            decks
        );
    }

    [HttpGet("{id}")]
    [ApiKey]
    // Get a deck by ID
    public async Task<IActionResult> GetDeckById(Guid id)
    {
        var user = (User)HttpContext.Items["User"]!;
        var deck = await DecksAdapter.GetDeck(_db, user.Id, id);

        if (deck == null)
        {
            return NotFound();
        }

        return Ok(deck);
    }

    [HttpPost("")]
    [ApiKey(ApiKeyScopes.Scope.DeckWrite)]
    // Create a new deck
    public async Task<IActionResult> CreateDeck(
        [FromBody] CreateDeckRequest request
    )
    {
        var user = (User)HttpContext.Items["User"]!;

        _logger.LogInformation("User: {}", user.Id);
        _logger.LogInformation("Creating deck: {}", request.Name);

        var deck = await DecksAdapter.CreateDeck(_db, user.Id, new DecksAdapter.CreateDeckRequest
        {
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Slug = request.Name.ToLower().Replace(" ", "-"),
            Tags = request.Tags,
            IsFeatured = request.IsFeatured ?? false,
            IsPublic = request.IsPublic ?? false
        });

        return Ok(deck);
    }

    public class CreateDeckRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string[]? Tags { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsPublic { get; set; }
    }
}
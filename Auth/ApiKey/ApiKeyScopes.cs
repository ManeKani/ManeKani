using ManeKani.Core.Models;

namespace ManeKani.Auth.ApiKey;

public static class ApiKeyScopes
{

    public enum Scope
    {
        DeckWrite,
        DeckDelete,
    }

    public static ApiKeyClaims IntoClaims(Scope[] scopes)
    {
        var claims = new ApiKeyClaims();

        foreach (var scope in scopes)
        {
            switch (scope)
            {
                case Scope.DeckWrite:
                    claims.DeckWrite = true;
                    break;
                case Scope.DeckDelete:
                    claims.DeckDelete = true;
                    break;
            }
        }

        return claims;
    }

}




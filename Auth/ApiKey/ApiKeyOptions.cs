using ManeKani.DB;
using Microsoft.AspNetCore.Authentication;

namespace ManeKani.Auth.ApiKey;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{


    public ApiKeyAuthenticationOptions()
    {
        ApiKeyCookie = "ApiKey";
    }


    public string ApiKeyCookie { get; set; }
}
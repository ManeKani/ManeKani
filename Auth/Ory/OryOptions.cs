using Microsoft.AspNetCore.Authentication;
using Ory.Client.Api;
using Ory.Client.Client;

namespace ManeKani.Auth.Ory;

public class OryAuthenticationOptions : AuthenticationSchemeOptions
{

    private FrontendApi _ory;
    protected string _oryBasePath;

    public OryAuthenticationOptions()
        : this(OryAuthenticationDefaults.OryBasePath)
    { }

    public OryAuthenticationOptions(string oryBasePath)
    {
        _ory = new FrontendApi(new Configuration
        {
            BasePath = oryBasePath,
        });
        _oryBasePath = oryBasePath;
    }

    public FrontendApi Ory { get => _ory; }
    public string? SessionKey { get; set; }
    public string OryBasePath
    {
        get =>
            _oryBasePath;
        set
        {
            _ory = new FrontendApi(new Configuration
            {
                BasePath = value
            });
            _oryBasePath = value;
        }
    }
}
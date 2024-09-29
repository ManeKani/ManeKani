namespace ManeKani.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/v1")]
[ApiController]
public class ManeKaniApiController : ControllerBase
{
    [Route("")]
    public IActionResult Index()
    {
        return Ok(new
        {
            message = "Welcome to the ManeKani API",
            version = "1.0.0"
        });
    }

    [Route("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            message = "Pong!"
        });
    }
}
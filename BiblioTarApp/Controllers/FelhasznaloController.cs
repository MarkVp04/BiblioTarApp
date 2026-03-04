using BiblioTarApp.Models;
using BiblioTarApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiblioTarApp.Controllers;

[ApiController]
[Route("[controller]")]
public class FelhasznaloController : ControllerBase
{
    private readonly IFelhasznaloService _felhasznaloService;

    public FelhasznaloController(IFelhasznaloService felhasznaloService)
    {
        _felhasznaloService = felhasznaloService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _felhasznaloService.Bejelentkezes(request.Email, request.Jelszo);
        if (user == null)
            return Unauthorized();

        var response = new LoginResponse
        {
            Id = user.Id,
            Nev = user.Nev,
            Email = user.Email,
            Beosztas = (int)user.Beosztas
        };
        return Ok(response);
    }
}

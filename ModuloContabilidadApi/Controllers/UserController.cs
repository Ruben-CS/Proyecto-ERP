using Microsoft.AspNetCore.Mvc;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Models;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public UserController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Usuario usuario)
    {
        var user = _dbContext.Usuarios.SingleOrDefault(u => u.Nombre ==
            usuario.Nombre && u.Password == usuario.Password);
        if (usuario is null)
        {
            return BadRequest("Usuario o contrasena invalidos");
        }

        return Ok("Login Exitoso");
    }
}
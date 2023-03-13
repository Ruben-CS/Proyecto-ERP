using Microsoft.AspNetCore.Mvc;

namespace ModuloContabilidadApi.Controllers;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
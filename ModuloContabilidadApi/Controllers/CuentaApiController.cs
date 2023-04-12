using Microsoft.AspNetCore.Mvc;

namespace ModuloContabilidadApi.Controllers;

public class CuentaApiController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
using Microsoft.AspNetCore.Mvc;

namespace ModuloContabilidadApi.Controllers;

public class EmpresaController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}
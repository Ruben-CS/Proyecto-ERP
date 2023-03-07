using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModuloContabilidadApi.Models;

namespace ModuloContabilidadApi.Controllers;

public class UsuarioController
{
    private readonly SignInManager<Usuario> _signInManager;

    public UsuarioController(SignInManager<Usuario> signInManager)
    {
        _signInManager = signInManager;
    }


}
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly APIResponse     _response;
    private          IAuthRepository _authRepository;


    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
        _response       = new APIResponse();
    }

    [HttpPost("login")]
    public async Task<IResult> Login([FromBody] LoginRequestDto modelo)
    {
        var response = new APIResponse()
        {
            IsSucces   = false,
            StatusCode = HttpStatusCode.BadRequest
        };

        var loginResponse = await _authRepository.Login(modelo);

        if (loginResponse is null)
        {
            return Results.BadRequest(response);
        }

        response.Result     = loginResponse;
        response.IsSucces   = true;
        response.StatusCode = HttpStatusCode.OK;
        return Results.Ok(response);
    }

    [HttpPost("registrar")]
    public async Task<IResult> Registrar(IAuthRepository _authRepository,
                                                 [FromBody] RegistroRequestDto modelo)
    {
        var response = new APIResponse()
        {
            IsSucces   = false,
            StatusCode = HttpStatusCode.BadRequest
        };

        var usuarioUnico = _authRepository.IsUnique(modelo.Nombre);

        if (!usuarioUnico)
        {
            response.ErrorMessages.Add("Usuario ya existe");
            return Results.BadRequest(response);
        }

        var registroResponse = await _authRepository.Registrar(modelo);

        if (registroResponse == null || string.IsNullOrEmpty(registroResponse.Nombre))
        {
            return Results.BadRequest(response);
        }
        
        response.IsSucces   = true;
        response.StatusCode = HttpStatusCode.OK;
        return Results.Ok(response);
    }
}
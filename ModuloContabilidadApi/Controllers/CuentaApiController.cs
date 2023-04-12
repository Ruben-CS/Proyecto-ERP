using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("cuentas")]
public class CuentaApiController : ControllerBase
{
    protected readonly ResponseDto       ResponseDto;
    private readonly   ICuentaRepository _cuentaRepository;

    public CuentaApiController(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
        ResponseDto       = new ResponseDto();
    }
}
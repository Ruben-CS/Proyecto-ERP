using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;
using Services.Validators;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("cuentas")]
public class CuentaApiController : ControllerBase
{
    private readonly ResponseDto          ResponseDto;
    private readonly ICuentaRepository    _cuentaRepository;
    public CuentaApiController(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
        ResponseDto       = new ResponseDto();
    }

    [HttpPost("agregarcuenta")]
    public async Task<object> PostCuenta([FromBody] CuentaDto cuenta)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.CreateCuenta(cuenta);
            await Task.FromResult(ResponseDto.Result = cuentaDto);
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(ResponseDto);
    }

    [HttpPut("actualizarcuenta/{id:int}")]
    public async Task<object> PutCuenta([FromBody] CuentaDto cuenta, int id)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.EditCuenta(cuenta, id);
            await Task.FromResult(ResponseDto.Result = cuentaDto);
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }
        return await Task.FromResult(ResponseDto);
    }
}
using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("cuentas")]
public class CuentaApiController : ControllerBase
{
    private readonly ResponseDto       _responseDto;
    private readonly ICuentaRepository _cuentaRepository;

    public CuentaApiController(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
        _responseDto      = new ResponseDto();
    }

    [HttpPost("agregarcuenta")]
    public async Task<object> PostCuenta([FromBody] CuentaDto cuenta)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.CreateCuenta(cuenta);
            await Task.FromResult(_responseDto.Result = cuentaDto);
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpPost("CrearCuentasPorDefecto/{IdEmpresa:int}")]
    public async Task<ResponseDto> CrearCuentasPorDefecto(
        [FromRoute] int IdEmpresa)
    {
        try
        {
            var cuentasCreadas = await _cuentaRepository.CreateDefaultCuentas(IdEmpresa);
            await Task.FromResult(_responseDto.Result = cuentasCreadas);
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess     = false;
            _responseDto.ErrorMessages = new List<string> { e.ToString() };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpPut("actualizarcuenta/{id:int}")]
    public async Task<object> PutCuenta([FromBody]  CuentaDto cuenta,
                                        [FromRoute] int       id)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.EditCuenta(cuenta, id);
            await Task.FromResult(_responseDto.Result = cuentaDto);
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpDelete("eliminarcuenta/{id:int}")]
    public async Task<object> DeleteCuenta([FromRoute] int id)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.DeleteCuenta(id);
            await Task.FromResult(_responseDto.Result = cuentaDto);
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpGet("getcuentas/{idempresa:int}")]
    public async Task<object> GetAllCuentas([FromRoute] int idempresa)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.GetAllCuentas(idempresa);
            await Task.FromResult(_responseDto.Result = cuentaDto);
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpGet("getCuentasDetalle/{idEmpresa:int}")]
    public async Task<object> GetCuentasDetalle([FromRoute] int idEmpresa)
    {
        try
        {
            var cuentasDetalle = await _cuentaRepository.GetCuentasTipoDetalle(idEmpresa);
            _responseDto.Result = cuentasDetalle;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return _responseDto;
    }
}
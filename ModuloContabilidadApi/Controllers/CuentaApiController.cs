using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("cuentas")]
public class CuentaApiController : ControllerBase
{
    private readonly ResponseDto          _responseDto;
    private readonly ICuentaRepository    _cuentaRepository;
    public CuentaApiController(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
        _responseDto       = new ResponseDto();
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

    [HttpPut("actualizarcuenta/{id:int}")]
    public async Task<object> PutCuenta([FromBody] CuentaDto cuenta, int id)
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
    public async Task<object> DeleteCuenta([FromBody] CuentaDto cuenta, int id)
    {
        try
        {
            var cuentaDto = await _cuentaRepository.DeleteCuenta(cuenta, id);
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
}
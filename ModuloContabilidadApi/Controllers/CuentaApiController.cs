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
    public async Task<object> CrearCuentasPorDefecto([FromRoute] int IdEmpresa)
    {
        var cuentasPorDefecto = new List<CuentaDto>
        {
            new() { Nombre = "Activo", TipoCuenta     = "Global", IdEmpresa = IdEmpresa },
            new() { Nombre = "Pasivo", TipoCuenta     = "Global", IdEmpresa = IdEmpresa },
            new() { Nombre = "Patrimonio", TipoCuenta = "Global", IdEmpresa = IdEmpresa },
            new() { Nombre = "Ingresos", TipoCuenta   = "Global", IdEmpresa = IdEmpresa },
            new() { Nombre = "Egresos", TipoCuenta    = "Global", IdEmpresa = IdEmpresa }
        };

        var cuentasCreadas = new List<CuentaDto>();

        foreach (var cuentaDefecto in cuentasPorDefecto)
        {
            try
            {
                var cuentaDto = await _cuentaRepository.CreateCuenta(cuentaDefecto);
                cuentasCreadas.Add(cuentaDto);
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>()
                {
                    e.ToString()
                };
                return _responseDto;
            }
        }

        // Crear las subcuentas para la cuenta 5 Egresos
        var cuentaEgresos = cuentasCreadas.Last();
        var subCuentasEgresos = new List<CuentaDto>
        {
            new()
            {
                Nombre    = "Costos", IdCuentaPadre = cuentaEgresos.IdCuenta,
                IdEmpresa = IdEmpresa
            },
            new()
            {
                Nombre    = "Gastos", IdCuentaPadre = cuentaEgresos.IdCuenta,
                IdEmpresa = IdEmpresa
            }
        };

        foreach (var subCuenta in subCuentasEgresos)
        {
            try
            {
                var cuentaDto = await _cuentaRepository.CreateCuenta(subCuenta);
                cuentasCreadas.Add(cuentaDto);
            }
            catch (Exception e)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>()
                {
                    e.ToString()
                };
                return _responseDto;
            }
        }

        _responseDto.Result = cuentasCreadas;
        return _responseDto;
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
}
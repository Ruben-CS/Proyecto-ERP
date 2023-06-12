using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("integracion")]
public class IntegracionApiController : ControllerBase
{
    private readonly IEmpresaRepository _empresaRepository;
    private readonly ResponseDto        _responseDto;


    public IntegracionApiController(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
        _responseDto       = new ResponseDto();
    }

    [HttpPut("agregarConfiguracion/{idEmpresa:int}")]
    public async Task<object> AgregarConfiguracion([FromBody] EmpresaDto dto,
                                                   int                   idEmpresa)
    {
        try
        {
            var gestionDto = await _empresaRepository.UpdateIntegracion(dto, idEmpresa);
            _responseDto.Result = gestionDto;
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

    [HttpPut("activarConfiguracion/{idEmpresa:int}")]
    public async Task<object> ActivarConfiguracion([FromBody] EmpresaDto dto,
                                                   int                   idEmpresa)
    {
        try
        {
            var gestionDto =
                await _empresaRepository.CambiarEstadoDeIntegracionTrue(dto, idEmpresa);
            _responseDto.Result = gestionDto;
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

    [HttpPut("desactivarConfiguracion/{idEmpresa:int}")]
    public async Task<object> DesactivarConfiguracion([FromBody] EmpresaDto dto,
                                                   int                   idEmpresa)
    {
        try
        {
            var gestionDto =
                await _empresaRepository.CambiarEstadoDeIntegracionFalse(dto, idEmpresa);
            _responseDto.Result = gestionDto;
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
using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace MonedaComprobanteApi.Controllers;

[ApiController]
[Route("comprobantes")]
public class ComprobanteController : ControllerBase
{
    private readonly IComprobanteRepository _comprobanteRepository;
    private readonly ResponseDto            _responseDto;

    public ComprobanteController(IComprobanteRepository comprobanteRepository)
    {
        _comprobanteRepository = comprobanteRepository;
        _responseDto           = new ResponseDto();
    }

    [HttpPost("/agregarcomprobante/{idEmpresa:int}")]
    public async Task<object> AgregarComprobante([FromBody] ComprobanteDto comprobanteDto,
                                                 [FromRoute] int idEmpresa)
    {
        try
        {
            var comprobante =
                await _comprobanteRepository.CrearComprobante(comprobanteDto, idEmpresa);
            _responseDto.Result = comprobante;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return _responseDto;
    }

    [HttpGet("/getcomprobantes/{idEmpresa:int}")]
    public async Task<object> ListarComprobantes(int idEmpresa)
    {
        try
        {
            var comprobantes = await _comprobanteRepository.GetAllComprobantes(idEmpresa);
            _responseDto.Result = comprobantes;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>
            {
                e.ToString()
            };
        }
        return _responseDto;
    }

    [HttpGet("/getComprobanteById/{idComprobante:int}")]
    public async Task<object> GetComprobante(int idComprobante)
    {
        try
        {
            var comprobantes = await _comprobanteRepository.GetCombrobanteById(idComprobante);
            _responseDto.Result = comprobantes;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>
            {
                e.ToString()
            };
        }
        return _responseDto;
    }
}
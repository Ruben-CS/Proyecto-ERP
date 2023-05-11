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
}
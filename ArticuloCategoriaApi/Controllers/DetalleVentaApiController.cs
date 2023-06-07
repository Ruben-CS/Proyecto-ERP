using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ArticuloCategoriaApi.Controllers;

[ApiController]
[Route("detalleVentas")]
public class DetalleVentaApiController : ControllerBase
{
    private readonly IDetalleRepository _detalleRepository;
    private readonly ResponseDto        _responseDto;

    public DetalleVentaApiController(IDetalleRepository detalleRepository)
    {
        _detalleRepository = detalleRepository;
        _responseDto       = new ResponseDto();
    }

    [HttpPost("agregarDetalleVenta")]
    public async Task<object> AgregarDetalleVenta([FromBody] DetalleDto dto)
    {
        try
        {
            var result = await _detalleRepository.AgregarDetalleVenta(dto);
            _responseDto.Result = result;
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

    [HttpGet("getDetalleVentas/{idNota:int}")]
    public async Task<object> AgregarDetalleVenta([FromRoute] int idNota)
    {
        try
        {
            var result = await _detalleRepository.ListarDetalles(idNota);
            _responseDto.Result = result;
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
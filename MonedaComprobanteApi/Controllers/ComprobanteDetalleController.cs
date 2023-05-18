using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace MonedaComprobanteApi.Controllers;

[ApiController]
[Route("detalleComprobantes")]
public class ComprobanteDetalleController : ControllerBase
{
    private readonly IDetalleComprobanteRepository _comprobanteRepository;
    private readonly ResponseDto                   _responseDto;

    public ComprobanteDetalleController(IDetalleComprobanteRepository comprobanteRepository)
    {
        _comprobanteRepository = comprobanteRepository;
        _responseDto           = new ResponseDto();
    }

    [HttpPost("agergarDetalleComprobante/{idComprobante:int}")]
    public async Task<object> AgregarDetalle([FromRoute] int idComprobante,
                                             DetalleComprobanteDto detalleComprobanteDto)
    {
        try
        {
            var detalleComprobante =
                await _comprobanteRepository.CreateDetalleComprobante(idComprobante, detalleComprobanteDto);
            _responseDto.Result = detalleComprobante;
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
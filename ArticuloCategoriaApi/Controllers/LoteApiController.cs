using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ArticuloCategoriaApi.Controllers;

[ApiController]
[Route("lotes")]
public class LoteApiController : ControllerBase
{
    private readonly ILoteRepository _loteRepository;
    private          ResponseDto     _responseDto;

    public LoteApiController(ILoteRepository loteRepository)
    {
        _loteRepository = loteRepository;
        _responseDto    = new ResponseDto();
    }

    [HttpPost("agregarLote/{idNota:int}")]
    public async Task<object> AgregarLote([FromBody]  LoteDto loteDto,
                                          [FromRoute] int     idNota)
    {
        try
        {
            var lote = await _loteRepository.CrearLote(loteDto, idNota);
            _responseDto.Result = lote;
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

    [HttpGet("getLotes/{idNota:int}")]
    public async Task<object> GetLotes([FromRoute] int idNota)
    {
        try
        {
            var lote = await _loteRepository.GetLotes(idNota);

            _responseDto.Result = lote;
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

    [HttpGet("getLotesPorArticulo/{idArticulo:int}")]
    public async Task<object> GetLotesPorArticulo([FromRoute] int idArticulo)
    {
        try
        {
            var lote = await _loteRepository.GetLotesPorArticulo(idArticulo);

            _responseDto.Result = lote;
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
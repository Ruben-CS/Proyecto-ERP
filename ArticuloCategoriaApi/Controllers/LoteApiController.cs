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

    [HttpPost("agregarLote/{idArticulo:int}")]
    public async Task<object> AgregarLote([FromBody]  LoteDto loteDto,
                                          [FromRoute] int     idArticulo)
    {
        try
        {
            var lote = await _loteRepository.CrearLote(loteDto, idArticulo);
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

    [HttpDelete("anularLote/{idLote:int}/{idArticulo:int}")]
    public async Task<object> AnularLote([FromRoute] int idLote,
                                         [FromRoute] int idArticulo)
    {
        try
        {
            var lote = await _loteRepository.EliminarLote(idLote, idArticulo);

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
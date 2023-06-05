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

    [HttpDelete("anularLote/{idLote:int}/{idNota:int}")]
    public async Task<object> AnularLote([FromRoute] int idNota)
    {
        try
        {
            var lote = await _loteRepository.EliminarLote(idNota);

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
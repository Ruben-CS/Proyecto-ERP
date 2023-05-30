using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ArticuloCategoriaApi.Controllers;

[ApiController]
[Route("notas")]
public sealed class NotaApiController : ControllerBase
{
    private readonly INotaRepository _notaRepository;
    private readonly ResponseDto     _responseDto;

    public NotaApiController(INotaRepository notaRepository)
    {
        _notaRepository = notaRepository;
        _responseDto    = new ResponseDto();
    }

    [HttpPost("agregarNota")]
    public async Task<object> PostNota([FromBody] NotaDto dto)
    {
        try
        {
            var nota = await _notaRepository.CreateNota(dto);
            _responseDto.Result = nota;
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

    [HttpGet("getNotas/{idEmpresa:int}")]
    public async Task<object> GetNotas([FromRoute] int idEmpresa)
    {
        try
        {
            var notas = await _notaRepository.GetNotas(idEmpresa);
            _responseDto.Result = notas;
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

    [HttpDelete("anularNota/{notaId:int}")]
    public async Task<object> DeleteArticulo([FromRoute] int notaId)
    {
        try
        {
            var result = await _notaRepository.AnularNota(notaId);
            _responseDto.Result = result;
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

    [HttpGet("getNota/{idNota:int}")]
    public async Task<object> GetNota([FromRoute] int idNota)
    {
        try
        {
            var nota = await _notaRepository.GetNota(idNota);
            _responseDto.Result = nota;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return _responseDto;
    }
}
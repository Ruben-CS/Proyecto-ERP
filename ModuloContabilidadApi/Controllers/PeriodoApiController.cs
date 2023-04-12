using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("periodos")]
public class PeriodoApiController : ControllerBase
{
    protected readonly ResponseDto ResponseDto;
    private readonly   IPeriodoRepository _periodoRepository;


    public PeriodoApiController(IPeriodoRepository periodoRepository)
    {
        _periodoRepository = periodoRepository;
        this.ResponseDto   = new ResponseDto();
    }

    [HttpGet("ListarPeriodos/{idgestion:int}")]
    public async Task<object> GetPeriodos([FromRoute] int idgestion)
    {
        try
        {
            var periodoDto = await _periodoRepository.GetModelos(idgestion);
            ResponseDto.Result = periodoDto;
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<object> Get(int id)
    {
        try
        {
            var periodoDto = await _periodoRepository.GetModelo(id);
            ResponseDto.Result = periodoDto;
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpPost("crearperiodo/{idGestion:int}")]
    public async Task<object> Post([FromBody]PeriodoDto periodoDto, [FromRoute] int idGestion)
    {
        try
        {
            var periodo = await _periodoRepository.CreateUpdateModelDto(periodoDto, idGestion);
            ResponseDto.Result = periodo;
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpPut("actualizarperiodo/{idGestion:int}/{idPeriodo:int}")]
    public async Task<object> Put([FromBody]PeriodoDto periodoDto,[FromRoute]
                                  int idGestion, int idPeriodo)
    {
        try
        {
            var periodo = await _periodoRepository.UpdateModel(periodoDto,idGestion, idPeriodo);
            ResponseDto.Result = periodo;
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpDelete("eliminarperiodo/{id:int}")]
    public async Task<object> Delete(int id)
    {
        try
        {
            var periodo = await _periodoRepository.DeleteModel(id);
            ResponseDto.Result = periodo;
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }
}
using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("gestiones")]
public class GestionApiController : ControllerBase
{
    protected readonly ResponseDto        ResponseDto;
    private readonly   IGestionRepository _gestionRepository;

    public GestionApiController(IGestionRepository gestionRepository)
    {
        ResponseDto        = new ResponseDto();
        _gestionRepository = gestionRepository;
    }

    [HttpGet("ListarGestion/id={id:int}")]
    public async Task<object> Get([FromRoute] int id)
    {
        try
        {
            var gestionDto = await _gestionRepository.GetModelos(id);
            ResponseDto.Result = gestionDto;
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

    [HttpPost("agregarGestion/{idEmpresa:int}")]
    public async Task<object> Post([FromBody]  GestionDto gestionDto,
                                   [FromRoute] int        idEmpresa)
    {
        try
        {
            var result =
                await _gestionRepository.CreateUpdateModelDto(gestionDto, idEmpresa);
            ResponseDto.Result = result;
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

    [HttpPut("actualizarGestion/{idGestion:int}")]
    public async Task<object> Put([FromBody] GestionDto gestionDto,
                                  [FromRoute] int idGestion)
    {
        try
        {
            var result =
                await _gestionRepository.UpdateModel(gestionDto, idGestion);
            ResponseDto.Result = result;
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

    [HttpDelete("eliminarGestion")]
    public async Task<object> Delete(int id)
    {
        try
        {
            var result = await _gestionRepository.DeleteModel(id);
            ResponseDto.Result = result;
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
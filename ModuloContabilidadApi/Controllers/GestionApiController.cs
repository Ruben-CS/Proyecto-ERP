using Microsoft.AspNetCore.Mvc;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;

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

    [HttpGet("ListarGestion")]
    public async Task<object> Get()
    {
        try
        {
            var gestionDto = await _gestionRepository.GetModelos();
            ResponseDto.Result = gestionDto;
        }
        catch (Exception e)
        {
            ResponseDto.IsSucces = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<object> Get(Guid id)
    {
        try
        {
            var gestionDto = await _gestionRepository.GetModelo(id);
            ResponseDto.Result = gestionDto;
        }
        catch (Exception e)
        {
            ResponseDto.IsSucces = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpPost("agregarGestion")]
    public async Task<object> Post([FromBody] GestionDto gestionDto)
    {
        try
        {
            var result =
                await _gestionRepository.CreateUpdateModelDto(gestionDto);
            ResponseDto.Result = result;
        }
        catch (Exception e)
        {
            ResponseDto.IsSucces = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpPut("actualizarGestion")]
    public async Task<object> Put([FromBody] GestionDto gestionDto)
    {
        try
        {
            var result =
                await _gestionRepository.CreateUpdateModelDto(gestionDto);
            ResponseDto.Result = result;
        }
        catch (Exception e)
        {
            ResponseDto.IsSucces = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }

    [HttpDelete("eliminarGestion")]
    public async Task<object> Delete(Guid id)
    {
        try
        {
            var result = await _gestionRepository.DeleteModel(id);
            ResponseDto.Result = result;
        }
        catch (Exception e)
        {
            ResponseDto.IsSucces = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return ResponseDto;
    }
}
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

    [HttpGet("ListarPeriodo")]
    public async Task<object> Get()
    {
        try
        {
            var periodoDto = await _periodoRepository.GetModelos();
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

    [HttpPost]
    public async Task<object> Post(PeriodoDto periodoDto)
    {
        try
        {
            var periodo = await _periodoRepository.CreateUpdateModelDto(periodoDto);
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

    [HttpPut]
    public async Task<object> Put(PeriodoDto periodoDto)
    {
        try
        {
            var periodo = await _periodoRepository.CreateUpdateModelDto(periodoDto);
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

    [HttpDelete]
    [Route("{id}")]
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
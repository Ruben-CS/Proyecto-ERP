using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("empresas")]
public class EmpresaApiController : ControllerBase
{
    protected readonly ResponseDto        ResponseDto;
    private readonly   IEmpresaRepository _empresaRepository;

    public EmpresaApiController(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
        ResponseDto        = new ResponseDto();
    }

    [HttpGet("ListarEmpresa")]
    public async Task<object> Get()
    {
        try
        {
            var empresaDto = await _empresaRepository.GetModelos();
            ResponseDto.Result = empresaDto;
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
            var empresaDto = await _empresaRepository.GetModelo(id);
            ResponseDto.Result = empresaDto;
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

    [HttpPost("agregarEmpresa")]
    public async Task<object> Post([FromBody] EmpresaDto empresa)
    {
        try
        {
            var empresaDto =
                await _empresaRepository.CreateUpdateModelDto(empresa);
            ResponseDto.Result = empresaDto;
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
    public async Task<object> Put([FromBody] EmpresaDto empresa)
    {
        try
        {
            var empresaDto =
                await _empresaRepository.UpdateModelDto(empresa);
            ResponseDto.Result = empresaDto;
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
    public async Task<object> Delete(int id)
    {
        try
        {
            var isSucces = await _empresaRepository.DeleteModel(id);
            ResponseDto.Result = isSucces;
        }
        catch (Exception e)
        {
            ResponseDto.IsSuccess = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
            throw;
        }

        return ResponseDto;
    }
}
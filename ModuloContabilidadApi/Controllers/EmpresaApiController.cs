using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("empresas")]
public class EmpresaApiController : ControllerBase
{
    private readonly ResponseDto        _responseDto;
    private readonly   IEmpresaRepository _empresaRepository;

    public EmpresaApiController(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
        _responseDto        = new ResponseDto();
    }

    [HttpGet("ListarEmpresa")]
    public async Task<object> Get()
    {
        try
        {
            var empresaDto = await _empresaRepository.GetModelos();
            _responseDto.Result = empresaDto;
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

    [HttpGet]
    [Route("{id}")]
    public async Task<object> Get(int id)
    {
        try
        {
            var empresaDto = await _empresaRepository.GetModelo(id);
            _responseDto.Result = empresaDto;
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

    [HttpPost("agregarEmpresa")]
    public async Task<object> Post([FromBody] EmpresaDto empresa)
    {
        try
        {
            var empresaDto =
                await _empresaRepository.CreateUpdateModelDto(empresa);
            _responseDto.Result = empresaDto;
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

    [HttpPut]
    public async Task<object> Put([FromBody] EmpresaDto empresa)
    {
        try
        {
            var empresaDto =
                await _empresaRepository.UpdateModelDto(empresa);
            _responseDto.Result = empresaDto;
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

    [HttpDelete]
    public async Task<object> Delete(int id)
    {
        try
        {
            var isSucces = await _empresaRepository.DeleteModel(id);
            _responseDto.Result = isSucces;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
            throw;
        }

        return _responseDto;
    }
}
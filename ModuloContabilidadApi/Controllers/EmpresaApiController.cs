using Microsoft.AspNetCore.Mvc;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Repository;

namespace ModuloContabilidadApi.Controllers;

[ApiController]
[Route("empresas")]
public class EmpresaApiController : ControllerBase
{
    protected readonly ResponseDto                ResponseDto;
    private readonly   IModeloRepository<Empresa> _empresaRepository;

    public EmpresaApiController(IModeloRepository<Empresa> empresaRepository)
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
            var empresaDto = await _empresaRepository.GetModelo(id);
            ResponseDto.Result = empresaDto;
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

    [HttpPost("agregarEmpresa")]
    public async Task<object> Post([FromBody] Empresa empresa)
    {
        try
        {
            var empresaDto =
                await _empresaRepository.CreateUpdateModelDto(empresa);
            ResponseDto.Result = empresaDto;
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

    [HttpPut]
    public async Task<object> Put([FromBody] Empresa empresa)
    {
        try
        {
            var empresaDto =
                await _empresaRepository.CreateUpdateModelDto(empresa);
            ResponseDto.Result = empresaDto;
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

    [HttpDelete]
    public async Task<object> Delete(Guid id)
    {
        try
        {
            var isSucces = await _empresaRepository.DeleteModel(id);
            ResponseDto.Result = isSucces;
        }
        catch (Exception e)
        {
            ResponseDto.IsSucces = false;
            ResponseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
            throw;
        }

        return ResponseDto;
    }
}
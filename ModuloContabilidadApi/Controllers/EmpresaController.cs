using Microsoft.AspNetCore.Mvc;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Repository;

namespace ModuloContabilidadApi.Controllers;




[ApiController]
[Route("api/empresas")]
public class EmpresaController : ControllerBase
{
    protected ResponseDto                _responseDto;
    private   IModeloRepository<Empresa> _empresaRepository;

    public EmpresaController(IModeloRepository<Empresa> empresaRepository)
    {
        _empresaRepository = empresaRepository;
        _responseDto       = new ResponseDto();
    }

    [HttpGet("GetEmpresa")]
    public async Task<object> Get()
    {
        try
        {
            var empresaDto = await _empresaRepository.GetModelos();
            _responseDto.Result = empresaDto;
        }
        catch (Exception e)
        {
            _responseDto.IsSucces = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }
        return _responseDto;
    }
    [HttpGet("GetEmpresaId")]
    [Route("{id}")]
    public async Task<object> Get(Guid id)
    {
        try
        {
            var empresaDto = await _empresaRepository.GetModelo(id);
            _responseDto.Result = empresaDto;
        }
        catch (Exception e)
        {
            _responseDto.IsSucces = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }
        return _responseDto;
    }

    [HttpPost("listarEmpresa")]
    public async Task<object> Post([FromBody] Empresa empresa)
    {
        try
        {
            var empresaDto = await _empresaRepository.CreateUpdateModelDto(empresa);
            _responseDto.Result = empresaDto;
        }
        catch (Exception e)
        {
            _responseDto.IsSucces = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }
        return _responseDto;
    }
    [HttpPut]
    public async Task<object> Put([FromBody] Empresa empresa)
    {
        try
        {
            var empresaDto = await _empresaRepository.CreateUpdateModelDto(empresa);
            _responseDto.Result = empresaDto;
        }
        catch (Exception e)
        {
            _responseDto.IsSucces = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }
        return _responseDto;
    }

    [HttpDelete]
    public async Task<object> Delete(Guid id)
    {
        try
        {
            var isSucces = await _empresaRepository.DeleteModel(id);
            _responseDto.Result = isSucces;
        }
        catch (Exception e)
        {
            _responseDto.IsSucces = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
            throw;
        }

        return _responseDto;
    }

}
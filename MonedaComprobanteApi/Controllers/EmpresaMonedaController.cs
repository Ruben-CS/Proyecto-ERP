using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace MonedaComprobanteApi.Controllers;

[ApiController]
[Route("empresaMonedas")]
public class EmpresaMonedaController : ControllerBase
{
    private readonly ResponseDto              _responseDto;
    private readonly IEmpresaMonedaRepository _empresaMonedaRepository;

    public EmpresaMonedaController(IEmpresaMonedaRepository empresaMonedaRepository)
    {
        _empresaMonedaRepository = empresaMonedaRepository;
        _responseDto             = new ResponseDto();
    }

    [HttpGet("getEmpresasMonedaByIdEmpresa/{id:int}")]
    public async Task<object> Get(int id)
    {
        try
        {
            var result = await _empresaMonedaRepository.GetEmpresasMonedas(id);
            _responseDto.Result = result;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>
            {
                e.ToString()
            };
            throw;
        }

        return _responseDto;
    }

    [HttpPost("agregarempresamoneda/{idEmpresa:int}/{idMoneda:int}")]
    public async Task<object> Post([FromBody]  EmpresaMonedaDto empresaMonedaDto,
                                   [FromRoute] int              idEmpresa,
                                   [FromRoute] int              idMoneda)
    {
        try
        {
            var result =
                await _empresaMonedaRepository.CreateEmpresaMoneda(empresaMonedaDto,
                    idEmpresa, idMoneda);
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

    [HttpGet("getActiveEmpresaMoneda/{idEmpresa:int}")]
    public async Task<object> GetActiveEmpresaMonedas([FromRoute] int idEmpresa)
    {
        try
        {
            var empresaMonedas = await _empresaMonedaRepository.GetMonedaAlternativasPerEmpresa(idEmpresa);
            _responseDto.Result = empresaMonedas;
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

    [HttpPatch("{id:int}")]
    public async Task<object> PatchEmpresaMoneda(
        int id, [FromBody] JsonPatchDocument<EmpresaMonedaDto> patchDoc)
    {
        try
        {
            var updatedEmpresaMoneda =
                await _empresaMonedaRepository.UpdateMoneda(patchDoc, id);
            _responseDto.Result = updatedEmpresaMoneda!;
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
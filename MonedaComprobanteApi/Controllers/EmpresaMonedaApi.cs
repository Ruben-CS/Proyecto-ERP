using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace MonedaComprobanteApi.Controllers;

[ApiController]
[Route("empresaMonedas")]
public class EmpresaMonedaApi : ControllerBase
{
    private readonly ResponseDto              _responseDto;
    private readonly IEmpresaMonedaRepository _empresaMonedaRepository;

    public EmpresaMonedaApi(IEmpresaMonedaRepository empresaMonedaRepository)
    {
        _empresaMonedaRepository = empresaMonedaRepository;
        _responseDto             = new ResponseDto();
    }

    [HttpGet("getEmpresasByIdEmpresa/{id:int}")]
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

    [HttpPost]
    public async Task<object> Post([FromBody] EmpresaMonedaDto empresaMonedaDto)
    {
        try
        {
            var result =
                await _empresaMonedaRepository.CreateEmpresaMoneda(empresaMonedaDto);
            _responseDto.Result = result;
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
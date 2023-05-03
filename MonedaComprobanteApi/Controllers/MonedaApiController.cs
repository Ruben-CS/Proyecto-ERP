using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace MonedaComprobanteApi.Controllers;

[ApiController]
[Route("monedas")]
public class MonedaApiController : ControllerBase
{
    private readonly ResponseDto       _responseDto;
    private readonly   IMonedaRepository _monedaRepository;

    public MonedaApiController(IMonedaRepository monedaRepository)
    {
        _monedaRepository = monedaRepository;
        _responseDto        = new ResponseDto();
    }


    [HttpGet("getMonedaById/{id:int}")]
    public async Task<object> Get(int id)
    {
        try
        {
            var moneda = await _monedaRepository.GetMoneda(id);
            _responseDto.Result = moneda;
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

    [HttpGet("getMonedas")]
    public async Task<object> Get()
    {
        try
        {
            var monedas = await _monedaRepository.GetAllMonedas();
            _responseDto.Result = monedas;
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
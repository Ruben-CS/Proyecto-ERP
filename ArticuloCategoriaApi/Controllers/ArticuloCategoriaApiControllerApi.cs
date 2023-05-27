using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ArticuloCategoriaApi.Controllers;

[ApiController]
[Route("articuloCategoria")]
public class ArticuloCategoriaApiControllerApi : ControllerBase
{
    private readonly IArticuloCategoriaRepository _articuloCategoriaRepository;
    private readonly ResponseDto                  _responseDto;

    public ArticuloCategoriaApiControllerApi(IArticuloCategoriaRepository
                                                 articuloCategoriaRepository)
    {
        _articuloCategoriaRepository = articuloCategoriaRepository;
        _responseDto                 = new ResponseDto();
    }


    [HttpPost("addArticuloCategoria/{idArticulo:int}/{idCategoria:int}")]
    public async Task<object> AddArticuloCat([FromBody] ArticuloCategoriaDto dto,
                                             int idArticulo, int idCategoria)
    {
        try
        {
            var categoria =
                await _articuloCategoriaRepository.CreateArticuloCategoria(dto,
                    idArticulo, idCategoria);
            _responseDto.Result = categoria;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }
    [HttpGet("getArticuloDetalles/{idArticulo:int}")]
    public async Task<object> GetArticuloDetalles(int idArticulo)
    {
        try
        {
            var articuloDetalles =
                await _articuloCategoriaRepository.GetArticuloDetalles(idArticulo);
            _responseDto.Result = articuloDetalles;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

}
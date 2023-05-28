using Microsoft.AspNetCore.Mvc;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace ArticuloCategoriaApi.Controllers;

[ApiController]
[Route("articulos")]
public class ArticuloApiController : ControllerBase
{
    private readonly ResponseDto         _responseDto;
    private readonly IArticuloRepository _articuloRepository;

    public ArticuloApiController(IArticuloRepository articuloRepository)
    {
        _articuloRepository = articuloRepository;
        _responseDto        = new ResponseDto();
    }

    [HttpGet("getaArticulos/{idEmpresa:int}")]
    public async Task<object> GetArticulos([FromRoute] int idEmpresa)
    {
        try
        {
            var categorias = await _articuloRepository.ListarArticulo(idEmpresa);
            _responseDto.Result = categorias;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpPost("agregarArticulo")]
    public async Task<object> GetArticulos([FromBody] ArticuloDto dto)
    {
        try
        {
            var categoria = await _articuloRepository.CrearArticulo(dto);
            _responseDto.Result = categoria;
        }
        catch (Exception e)
        {
            _responseDto.IsSuccess = false;
            _responseDto.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
        }

        return await Task.FromResult(_responseDto);
    }

    [HttpDelete("eliminarArticulo/{id:int}")]
    public async Task<object> DeleteArticulo([FromRoute] int id)
    {
        try
        {
            var cuentaDto = await _articuloRepository.BorrarArticulo(id);
            await Task.FromResult(_responseDto.Result = cuentaDto);
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

    [HttpPut("editarArticulo/{idArticulo:int}")]
    public async Task<object> EditArticulo([FromBody]  ArticuloDto articuloDto,
                                            [FromRoute] int          idArticulo)
    {
        try
        {
            var categoria =
                await _articuloRepository.EditarArticulo(articuloDto, idArticulo);
            await Task.FromResult(_responseDto.Result = categoria);
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

    [HttpGet("getArticulo/{idArticulo:int}")]
    public async Task<object> EditArticulo([FromRoute] int idArticulo)
    {
        try
        {
            var articulo =
                await _articuloRepository.GetSingleArticulo(idArticulo);
            await Task.FromResult(_responseDto.Result = articulo);
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
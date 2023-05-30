using Modelos.Models.Dtos;using Microsoft.AspNetCore.Mvc;

using Services.Repository.Interfaces;

namespace ArticuloCategoriaApi.Controllers;

[Route("categorias")]
[ApiController]
public class CategoriaApiController : ControllerBase
{
    private readonly ResponseDto          _responseDto;
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaApiController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
        _responseDto        = new ResponseDto();
    }


    [HttpGet("getCategorias/{idEmpresa:int}")]
    public async Task<object> GetCategorias([FromRoute] int idEmpresa)
    {
        try
        {
            var categorias = await _categoriaRepository.ListarCategoria(idEmpresa);
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

    [HttpPost("agregarCategoria")]
    public async Task<object> GetCategorias([FromBody] CategoriaDto dto)
    {
        try
        {
            var categoria = await _categoriaRepository.CreateCategoria(dto);
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

    [HttpDelete("eliminarCategoria/{id:int}")]
    public async Task<object> DeleteCuenta([FromRoute] int id)
    {
        try
        {
            var cuentaDto = await _categoriaRepository.EliminarDto(id);
            await Task.FromResult(_responseDto.Result = cuentaDto);
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

    [HttpPut("editarCategoria/{idCategoria:int}")]
    public async Task<object> EditCategoria([FromBody]  CategoriaDto categoriaDto,
                                            [FromRoute] int          idCategoria)
    {
        try
        {
            var categoria =
                await _categoriaRepository.EditarDto(categoriaDto, idCategoria);
            await Task.FromResult(_responseDto.Result = categoria);
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
}
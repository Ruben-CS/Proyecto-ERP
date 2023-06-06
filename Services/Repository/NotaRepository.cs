using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class NotaRepository : INotaRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper              _mapper;

    public NotaRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper    = mapper;
    }

    public async Task<NotaDto> CreateNota(NotaDto notaDto)
    {
        var nota = _mapper.Map<NotaDto, Nota>(notaDto);
        _dbContext.Add(nota);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Nota, NotaDto>(nota);
    }

    public async Task<bool> AnularNota(int notaId)
    {
        var nota = await _dbContext.Nota.FirstOrDefaultAsync(n => n.IdNota == notaId);

        var lotes = await _dbContext.Lotes.Where(x => x.IdNota == notaId).ToListAsync();

        lotes.ForEach(l =>
        {
            var articulo =
                _dbContext.Articulo.Single(a => a.IdArticulo == l.IdArticulo);
            articulo.Cantidad -= l.Cantidad;
            l.EstadoLote      =  EstadoLote.Anulado;
        });

        if (nota is null)
        {
            return false;
        }

        nota.EstadoNota = EstadoNota.Anulado;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<NotaDto>> GetNotaCompra(int idEmpresa)
    {
        var notaCompra = await _dbContext.Nota
                                         .AsNoTracking()
                                         .Where(n =>
                                             n.IdEmpresa == idEmpresa &&
                                             n.TipoNota  == TipoNota.Compra)
                                         .Select(n => _mapper.Map<NotaDto>(n))
                                         .ToListAsync();

        return notaCompra;
    }

    public async Task<IEnumerable<NotaDto>> GetNotaVenta(int idEmpresa)
    {
        var notaVenta = await _dbContext.Nota.AsNoTracking().Where(n =>
                                            n.IdEmpresa == idEmpresa &&
                                            n.TipoNota  == TipoNota.Venta).Select(n =>
                                            _mapper.Map<NotaDto>(n))
                                        .ToListAsync();
        return notaVenta;
    }

    public async Task<NotaDto> GetNota(int notaId)
    {
        var nota = await _dbContext.Nota
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(n => n.IdNota == notaId);

        if (nota == null)
        {
            throw new Exception("Nota not found");
        }

        var notaDto = _mapper.Map<NotaDto>(nota);

        return notaDto;
    }
}
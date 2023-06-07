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

    public async Task<bool> AnularNota(int idNota)
    {
        var nota = await _dbContext.Nota.FirstOrDefaultAsync(n => n.IdNota == idNota);

        var lotes = await _dbContext.Lotes.Where(x => x.IdNota == idNota).ToListAsync();

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

    public async Task<NotaDto> GetNota(int idNota)
    {
        var nota = await _dbContext.Nota
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(n => n.IdNota == idNota);

        if (nota == null)
        {
            throw new Exception("Nota not found");
        }

        var notaDto = _mapper.Map<NotaDto>(nota);

        return notaDto;
    }

    //todo optimize this code
    public async Task<bool> AnularNotaVenta(int idNota)
    {
        var notaDeVenta =
            await _dbContext.Nota.SingleOrDefaultAsync(x => x.IdNota == idNota);
        var detalleDeVenta =
            await _dbContext.Detalle.Where(x => x.IdNota == idNota).ToListAsync();

        notaDeVenta!.EstadoNota = EstadoNota.Anulado;

        foreach (var detalle in detalleDeVenta)
        {
            var restablecerCantidad = detalle.Cantidad;
            var articulo =
                await _dbContext.Articulo.SingleOrDefaultAsync(g =>
                    g.IdArticulo == detalle.IdArticulo);
            var lote = await _dbContext.Lotes.SingleOrDefaultAsync(g =>
                g.NroLote == detalle.NroLote && g.IdArticulo == detalle.IdArticulo);

            if (articulo != null)
            {
                articulo.Cantidad                += restablecerCantidad;
                _dbContext.Entry(articulo).State =  EntityState.Modified;
            }

            if (lote != null)
            {
                lote.Stock += restablecerCantidad;
                lote.EstadoLote = lote.Stock < 1 ? EstadoLote.Agotado : EstadoLote.Activo;
                _dbContext.Entry(lote).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }

        _dbContext.Entry(notaDeVenta).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
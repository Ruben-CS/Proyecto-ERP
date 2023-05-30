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
        if (nota is null)
        {
            return false;
        }

        nota.EstadoNota = EstadoNota.Anulado;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<NotaDto>> GetNotas(int idEmpresa)
    {
        // Use AsNoTracking() and project directly onto NotaDto
        var notas = await _dbContext.Nota
                                    .AsNoTracking()
                                    .Where(n => n.IdEmpresa == idEmpresa)
                                    .Select(n => _mapper.Map<NotaDto>(n))
                                    .ToListAsync();

        return notas;
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
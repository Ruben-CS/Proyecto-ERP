using AutoMapper;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;

namespace ModuloContabilidadApi;

public class MappingConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<EmpresaDto, Empresa>();
            config.CreateMap<Empresa, EmpresaDto>();

            config.CreateMap<GestionDto, Gestion>();
            config.CreateMap<Gestion, GestionDto>();

            config.CreateMap<PeriodoDto, Periodo>();
            config.CreateMap<Periodo, PeriodoDto>();
        });
        return mappingConfig;
    }
}
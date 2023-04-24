using AutoMapper;
using Modelos.Models;
using Modelos.Models.Dtos;

namespace ModuloContabilidadApi;

public static class MappingConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<EmpresaDto, Empresa>();
            config.CreateMap<Empresa, EmpresaDto>();

            config.CreateMap<GestionDto, Gestion>()
                  .ForMember(dest => dest.IdEmpresa, opt => opt.Ignore());
            config.CreateMap<Gestion, GestionDto>();

            config.CreateMap<PeriodoDto, Periodo>();
            config.CreateMap<Periodo, PeriodoDto>();

            config.CreateMap<UsuarioDto, Usuario>();
            config.CreateMap<Usuario, UsuarioDto>();

            config.CreateMap<CuentaDto, Cuenta>()
                  .ForMember(dest => dest.IdCuenta, opt => opt.Ignore());
            config.CreateMap<Cuenta, CuentaDto>();
        });
        return mappingConfig;
    }
}
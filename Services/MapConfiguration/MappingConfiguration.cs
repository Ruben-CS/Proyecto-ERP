using AutoMapper;
using Modelos.Models;
using Modelos.Models.Dtos;

namespace Services.MapConfiguration;

public static class MappingConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<EmpresaDto, Empresa>();
            config.CreateMap<Empresa, EmpresaDto>();

            config.CreateMap<GestionDto, Modelos.Models.Gestion>()
                  .ForMember(dest => dest.IdEmpresa, opt => opt.Ignore());
            config.CreateMap<Modelos.Models.Gestion, GestionDto>();

            config.CreateMap<PeriodoDto, Modelos.Models.Periodo>();
            config.CreateMap<Modelos.Models.Periodo, PeriodoDto>();

            config.CreateMap<UsuarioDto, Usuario>();
            config.CreateMap<Usuario, UsuarioDto>();

            config.CreateMap<CuentaDto, Modelos.Models.Cuenta>()
                  .ForMember(dest => dest.IdCuenta, opt => opt.Ignore());
            config.CreateMap<Modelos.Models.Cuenta, CuentaDto>();

            config.CreateMap<MonedaDto, Modelos.Models.Moneda>();
            config.CreateMap<Modelos.Models.Moneda, MonedaDto>();

            config.CreateMap<EmpresaMonedaDto, EmpresaMoneda>();
            config.CreateMap<EmpresaMoneda, EmpresaMonedaDto>();

            config.CreateMap<ComprobanteDto, Modelos.Models.Comprobante>();
            config.CreateMap<Modelos.Models.Comprobante, ComprobanteDto>();

            config.CreateMap<DetalleComprobante, DetalleComprobanteDto>();
            config.CreateMap<DetalleComprobanteDto, DetalleComprobante>();
        });
        return mappingConfig;
    }
}
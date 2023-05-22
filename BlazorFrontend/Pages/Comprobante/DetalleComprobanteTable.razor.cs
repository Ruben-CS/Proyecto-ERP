using System.Collections.ObjectModel;
using BlazorFrontend.Pages.Comprobante.Editar;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class DetalleComprobanteTable
{

    [Parameter]
    public ObservableCollection<DetalleComprobanteDto> Detalles { get; set; } = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public List<CuentaDto> Cuentas { get; set; } = null!;


    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    private decimal TotalDebe => Detalles.Sum(x => x.MontoDebe);

    private decimal TotalHaber => Detalles.Sum(x => x.MontoHaber);

    private void DeleteDetalle(DetalleComprobanteDto dto) => Detalles.Remove(dto);


    private async Task ShowEditDetalleModal(DetalleComprobanteDto dto)
    {
        var indexOfDto = Detalles.IndexOf(dto);

        var parameters = new DialogParameters
        {
            { "DetalleComprobanteDto", dto },
            { "IndexOfDetalle", indexOfDto },
            { "IdEmpresa", IdEmpresa },
            { "Detalles", Detalles  }
        };
        await DialogService.ShowAsync<EditarDetalle>(
            "Edite los detalles del comprobante", parameters,
            _options);
    }

}
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class DetalleComprobanteTable
{
    private MudTable<DetalleComprobanteDto> table;


    [Parameter]
    public ObservableCollection<DetalleComprobanteDto> Detalles { get; set; } = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public List<CuentaDto> Cuentas { get; set; } = null!;

    private DetalleComprobanteDto elementBeforeEdit;

    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    private decimal TotalDebe => Detalles.Sum(x => x.MontoDebe);

    private decimal TotalHaber => Detalles.Sum(x => x.MontoHaber);

    private void DeleteDetalle(DetalleComprobanteDto dto) => Detalles.Remove(dto);


    private async Task<IEnumerable<string>> SearchCuenta(string value)
    {
        Cuentas = await CuentaService.GetCuentasDetalle(IdEmpresa);
        IEnumerable<string> nombreCuentas =
            Cuentas.Select(c => $"{c.Codigo} - {c.Nombre}").ToList();
        if (string.IsNullOrEmpty(value))
        {
            return await Task.FromResult(nombreCuentas);
        }

        return nombreCuentas.Where(c =>
            c.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }


    private void OnRowEditPreview(object detalleObj)
    {
        var detalle = detalleObj as DetalleComprobanteDto;
        elementBeforeEdit = new DetalleComprobanteDto
        {
            NombreCuenta = detalle.NombreCuenta,
            Glosa        = detalle.Glosa,
            MontoDebe    = detalle.MontoDebe,
            MontoHaber   = detalle.MontoHaber
        };
    }

    private void OnRowEditCommit(object detalleObj)
    {
        StateHasChanged();
    }

    private void OnRowEditCancel(object detalleObj)
    {
        var detalle = detalleObj as DetalleComprobanteDto;
        detalle.NombreCuenta = elementBeforeEdit.NombreCuenta;
        detalle.Glosa        = elementBeforeEdit.Glosa;
        detalle.MontoDebe    = detalle.MontoDebe;
        detalle.MontoHaber   = elementBeforeEdit.MontoHaber;
    }
}
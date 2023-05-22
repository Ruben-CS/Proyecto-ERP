using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante.Editar;

public partial class EditarDetalle
{
    [Parameter]
    public DetalleComprobanteDto DetalleComprobanteDto { get; set; } = new();

    [Parameter]
    public ObservableCollection<DetalleComprobanteDto> Detalles { get; set; } = new();

    [Parameter]
    public int IndexOfDetalle { get; set; }

    [Parameter]
    public int IdEmpresa { get; set; }

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    private List<CuentaDto> Cuentas        { get; set; } = new();
    private string?         SelectedCuenta { get; set; }

    private void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        Cuentas = await CuentaService.GetCuentasDetalle(IdEmpresa);
    }

    private async Task<IEnumerable<string>> SearchCuenta(string value)
    {
        IEnumerable<string> nombreCuentas =
            Cuentas.Select(c => $"{c.Codigo} - {c.Nombre}").ToList();
        if (string.IsNullOrEmpty(value))
        {
            return await Task.FromResult(nombreCuentas);
        }

        return nombreCuentas.Where(c =>
            c.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private void Editar()
    {
        var selectedCuentaCodigo = ExtractNumber(SelectedCuenta!);
        var idCuenta = Cuentas.SingleOrDefault(c => c.Codigo == selectedCuentaCodigo)!
                              .IdCuenta;
        var editedDetalle = new DetalleComprobanteDto
        {
            Glosa         = DetalleComprobanteDto.Glosa,
            IdCuenta      = idCuenta,
            MontoHaber    = DetalleComprobanteDto.MontoHaber,
            MontoDebe     = DetalleComprobanteDto.MontoDebe,
            MontoDebeAlt  = default,
            MontoHaberAlt = default,
            IdUsuario     = 1
        };
        DetalleComprobanteDto    = editedDetalle;
        Detalles[IndexOfDetalle] = editedDetalle;
        StateHasChanged();
    }

    private string ExtractNumber(string nombreCuenta)
    {
        var separatorIndex = nombreCuenta.IndexOf(" - ", StringComparison.Ordinal);

        if (separatorIndex == -1)
            return string.Empty;
        var numberPart = nombreCuenta[..separatorIndex].Trim();

        return IsValidNumberPart(numberPart) ? numberPart : string.Empty;
    }

    private static bool IsValidNumberPart(string numberPart)
    {
        var segments = numberPart.Split('.');
        if (segments.Any(segment => !int.TryParse(segment, out _)))
        {
            return false;
        }

        return segments.Length >= 3 && segments.Length <= 7;
    }
}
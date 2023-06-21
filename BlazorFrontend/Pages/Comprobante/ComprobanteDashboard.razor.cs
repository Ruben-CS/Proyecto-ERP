using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class ComprobanteDashboard
{
    private MudTable<ComprobanteDto> _table;

    [Parameter]
    public int IdEmpresa { get; set; }

    private List<ComprobanteDto> Comprobantes       { get; set; } = new();
    private void                 PageChanged(int i) => _table.NavigateTo(i - 1);

    private bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa = int.Parse(idValue);
                Comprobantes = await ComprobanteService.GetComprobantesAsync(IdEmpresa);
                await InvokeAsync(StateHasChanged);
                IsLoading = false;
            }
            else
            {
                throw new KeyNotFoundException(
                    "The 'id' parameter was not found in the query string.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An error occurred while initializing the component: {ex}");
        }
    }

    private void NavigateToDetalleComprobante(int idcomprobante, int idempresa)
    {
        var uri = $"/VerDetallesComprobante/{idempresa}/{idcomprobante}";
        NavigationManager!.NavigateTo(uri);
    }

    private void GoToDetalle()
    {
        var uri = $"/comprobantegrid/{IdEmpresa}";
        NavigationManager!.NavigateTo(uri);
    }
}
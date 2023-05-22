using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Comprobante;

public partial class ComprobanteDashboard
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private List<ComprobanteDto> Comprobantes { get; set; } = new();

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
                await base.OnInitializedAsync();
                Comprobantes = await ComprobanteService.GetComprobantesAsync(IdEmpresa);
                IdEmpresa    = int.Parse(idValue);
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
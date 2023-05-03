using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Moneda;

public partial class MonedaDashboard
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private List<EmpresaMonedaDto> _empresaMonedas = new();
    private List<MonedaDto>        _monedas        = new();

    public  MonedaDto    MonedaPrincipal          { get; set; }
    private bool IsExpanded { get; set; }

    private bool _open;
    private void ToggleDrawer()   => _open = !_open;
    private void CambiarEmpresa() => NavigationManager.NavigateTo("/inicio");

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa = int.Parse(idValue);
                _empresaMonedas =
                    await EmpresaMonedaService.GetEmpresasMonedaAsync(IdEmpresa);
                _monedas        = (await MonedaService.GetMonedasAsync())!;
                MonedaPrincipal = await GetMonedaPrincipal();
                await InvokeAsync(StateHasChanged);
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

    private async Task<MonedaDto?> GetMonedaPrincipal()
    {
        var empresaMonedaDto = _empresaMonedas.Find(em => em.IdEmpresa == IdEmpresa);

        var monedaPrincipal = await MonedaService.
            GetMonedaByIdAsync(empresaMonedaDto.IdMonedaPrincipal) ?? default;
        return monedaPrincipal;
    }


    private void NavigateToCuentas()
    {
        if (IdEmpresa is 0)
            return;
        var uri = $"/plandecuentas/overview/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }

    private void NavigateToGestiones()
    {
        if (IdEmpresa is 0)
            return;
        var uri = $"/gestion/overview/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }

    private void NavigateToMonedas()
    {
        if (IdEmpresa is 0)
            return;
        var uri = $"/inicio/configuracion/monedaDashboard/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }
}
using System.Text;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;
using Newtonsoft.Json;

namespace BlazorFrontend.Pages.Moneda;

public partial class MonedaDashboard
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private List<EmpresaMonedaDto> _empresaMonedas = new();
    private List<MonedaDto>        _monedas        = new();

    private EmpresaMonedaDto EmpresaMonedaDto { get; set; } = new();
    private MonedaDto        MonedaPrincipal  { get; set; } = null!;

    public  MonedaDto MonedaDto  { get; set; } = new();
    private bool      IsExpanded { get; set; }

    public float? Cambio { get; set; } = new();

    private string? _previousSelectedMoneda;


    private string MonedaPrincipalName { get; set; }

    private string? SelectedMoneda { get; set; }

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
                MonedaPrincipal = (await GetMonedaPrincipal())!;
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

    protected override void OnParametersSet()
    {
        if (SelectedMoneda == _previousSelectedMoneda) return;
        _previousSelectedMoneda = SelectedMoneda;
        SelectedMoneda = _monedas.FirstOrDefault(m => m.Nombre == SelectedMoneda)?.Nombre;
        MonedaDto = _monedas.Single(m => m.Nombre == SelectedMoneda);
    }

    private async Task<MonedaDto?> GetMonedaPrincipal()
    {
        var empresaMonedaDto = _empresaMonedas.Find(em => em.IdEmpresa == IdEmpresa);

        var monedaPrincipal =
            await MonedaService.GetMonedaByIdAsync(empresaMonedaDto.IdMonedaPrincipal) ??
            default;
        MonedaPrincipalName = monedaPrincipal.Nombre;
        return monedaPrincipal;
    }

    private async Task AddMonedaAlternativa()
    {
        var selectedMonedaAlternativa =
            _monedas.FirstOrDefault(ma => ma.Nombre == SelectedMoneda);
        var idMonedaAlternativa = selectedMonedaAlternativa!.IdMoneda;
        //Todo check why cambio is null
        // var cambio               = EmpresaMonedaDto.Cambio;
        float? cambio               = Cambio;
        var    currentEmpresaMoneda = _empresaMonedas[0];
        //TODO Retrieve the primary key of the current empresa moneda
        var url =
            $"https://localhost:44352/empresaMonedas/{currentEmpresaMoneda.IdEmpresaMoneda}";

        var patchOperations = new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                { "op", "replace" },
                { "path", "/cambio" },
                { "value", cambio }
            },
            new Dictionary<string, object>
            {
                { "op", "replace" },
                { "path", "/idMonedaAlternativa" },
                { "value", idMonedaAlternativa }
            }
        };
        var content = new StringContent(JsonConvert.SerializeObject(patchOperations),
            Encoding.UTF8, "application/json");
        var response = await HttpClient.PatchAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("EmpresaMoneda actualizada exitosamente", Severity.Success);
            // Perform any additional actions required after a successful update
        }
    }

    private string GetMonedaPrincipalName() => MonedaPrincipal.Nombre;


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
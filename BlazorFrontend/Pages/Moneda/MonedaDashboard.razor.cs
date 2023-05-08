using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Moneda;

public partial class MonedaDashboard
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private List<EmpresaMonedaDto> _empresaMonedas = new();
    private List<MonedaDto>        _monedas        = new();
    private bool                   IsLoading { get; set; } = true;

    private EmpresaMonedaDto EmpresaMonedaDto { get; }      = new();
    private MonedaDto        MonedaPrincipal  { get; set; } = null!;

    private bool IsExpanded { get; set; }

    private float? Cambio => AppState.Cambio;

    private string? _previousSelectedMoneda;
    private string? MonedaPrincipalName { get; set; }

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
                IsLoading       = false;
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
    }

    private async Task<MonedaDto?> GetMonedaPrincipal()
    {
        var empresaMonedaDto = _empresaMonedas.First();

        var monedaPrincipal =
            await MonedaService.GetMonedaByIdAsync(empresaMonedaDto.IdMonedaPrincipal) ??
            default;
        MonedaPrincipalName = monedaPrincipal?.Nombre;
        return monedaPrincipal;
    }

    private async Task AddMonedaAlternativa()
    {
        var selectedMonedaAlternativa =
            _monedas.FirstOrDefault(ma => ma.Nombre == SelectedMoneda);
        var idMonedaAlternativa = selectedMonedaAlternativa!.IdMoneda;
        var cambio              = Cambio;
        var url =
            $"https://localhost:44352/empresaMonedas/agregarempresamoneda/{IdEmpresa}/{idMonedaAlternativa}";


        var empresaMonedaDto = new EmpresaMonedaDto
        {
            Cambio              = cambio,
            IdEmpresa           = IdEmpresa,
            IdMonedaPrincipal   = MonedaPrincipal.IdMoneda,
            IdMonedaAlternativa = idMonedaAlternativa,
            IdUsuario           = 1
        };
        if (await ValidateIncorrectCambio(empresaMonedaDto.Cambio))
        {
            Snackbar.Add("Tipo de cambio incorrecto", Severity.Error);
        }
        else if (await ValidateTipoDeCambio(empresaMonedaDto.Cambio))
        {
            Snackbar.Add("Ya existe una moneda con este tipo de cambio", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, empresaMonedaDto);
            if (response.IsSuccessStatusCode)
            {
                await OnDataGridChange();
                Snackbar.Add("Moneda agregada exitosamente", Severity.Success);
            }
        }
    }

    private string GetMonedaAlternativaName(int? idMonedaAlternativa)
    {
        var monedaAlterna = _monedas.SingleOrDefault(ma =>
            ma.IdMoneda == idMonedaAlternativa);
        return monedaAlterna?.Nombre ?? string.Empty;
    }

    private static async Task<bool> ValidateIncorrectCambio(float? cambio) =>
        await Task.FromResult(cambio is null ||
                              float.IsNegative((float)cambio));


    private async Task<bool> ValidateTipoDeCambio(float? cambio)
    {
        return await Task.FromResult(
            _empresaMonedas.Any(em => Equals(em.Cambio, cambio)));
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

    private async Task OnDataGridChange()
    {
        _empresaMonedas = await EmpresaMonedaService.GetEmpresasMonedaAsync(IdEmpresa);
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }

    private void NavigateToMonedas()
    {
        if (IdEmpresa is 0)
            return;
        var uri = $"/inicio/configuracion/monedaDashboard/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }
}
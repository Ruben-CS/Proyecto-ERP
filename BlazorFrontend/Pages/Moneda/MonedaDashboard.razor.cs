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

    private decimal? Cambio
    {
        get => AppState.Cambio;
        set => AppState.Cambio = value;
    }

    private string? _previousSelectedMoneda;
    private string? MonedaPrincipalName { get; set; }

    private string? SelectedMoneda { get; set; }
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

        if (selectedMonedaAlternativa is null)
        {
            Snackbar.Add("Seleccione una moneda alternativa", Severity.Error);
            return;
        }

        var idMonedaPrincial    =  MonedaPrincipal.IdMoneda;
        var idMonedaAlternativa = selectedMonedaAlternativa.IdMoneda;
        var cambio              = Cambio;
        var url =
            $"https://localhost:44352/empresaMonedas/agregarmonedaAlternativa/{IdEmpresa}/{idMonedaAlternativa}/{idMonedaPrincial}";


        var empresaMonedaDto = new EmpresaMonedaDto
        {
            Cambio              = cambio,
            IdEmpresa           = IdEmpresa,
            IdMonedaPrincipal   = idMonedaPrincial,
            IdMonedaAlternativa = idMonedaAlternativa,
            IdUsuario           = 1
        };
        if (SelectedMoneda is null)
        {
            Snackbar.Add("Seleccione una moneda", Severity.Error);
        }
        else if (await ValidateIncorrectCambio(empresaMonedaDto.Cambio))
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

            SelectedMoneda = null;
            Cambio         = null;
        }
    }

    private string GetMonedaAlternativaName(int? idMonedaAlternativa)
    {
        var monedaAlterna = _monedas.SingleOrDefault(ma =>
            ma.IdMoneda == idMonedaAlternativa);
        return monedaAlterna?.Nombre ?? string.Empty;
    }

    private static async Task<bool> ValidateIncorrectCambio(decimal? cambio) =>
        await Task.FromResult(cambio is null ||
                              decimal.IsNegative((decimal)cambio));


    private async Task<bool> ValidateTipoDeCambio(decimal? cambio)
    {
        return await Task.FromResult(
            _empresaMonedas.Any(em => Equals(em.Cambio, cambio)));
    }
    private async Task OnDataGridChange()
    {
        _empresaMonedas = await EmpresaMonedaService.GetEmpresasMonedaAsync(IdEmpresa);
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }

}
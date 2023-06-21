using Microsoft.AspNetCore.Components;
using MudBlazor;
using Modelos.Models.Dtos;
using BlazorFrontend.Pages.Empresa.Crear;
using BlazorFrontend.Pages.Empresa.Editar;
using BlazorFrontend.Pages.Empresa.Eliminar;
using Microsoft.JSInterop;
using DialogOptions = MudBlazor.DialogOptions;

namespace BlazorFrontend.Pages.Empresa;

public partial class Inicio
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null !;

    private IEnumerable<EmpresaDto> _empresas = new List<EmpresaDto>();
    private int                     SelectedEmpresaId { get; set; }

    private string? Username { get; set; }

    private string? SelectedEmpresaName { get; set; }

    private bool IsLoading { get; set; }

    private bool IsSelectedEmpresaNameNull => SelectedEmpresaName is null;

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    private async Task LoadSelectedEmpresaAsync(int selectedId)
    {
        SelectedEmpresaId = selectedId;
        await InvokeAsync(StateHasChanged);
    }

    private async Task<IEnumerable<string>> Search(string value)
    {
        IEnumerable<string> empresasName = _empresas.Select(x => x.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
        {
            return empresasName;
        }

        return await Task.FromResult(empresasName.Where(e =>
            e.Contains(value, StringComparison.InvariantCultureIgnoreCase)));
    }

    private async Task ShowMudCrearEmpresaModal()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey     = true,
            MaxWidth             = MaxWidth.Small,
            FullWidth            = true,
            DisableBackdropClick = true,
        };
        var parameters = new DialogParameters
        {
            {
                "OnEmpresaListChange",
                EventCallback.Factory.Create<EmpresaDto>(this, OnEmpresaListChange)
            }
        };
        await DialogService.ShowAsync<CrearEmpresa>
            ("Llene los datos de la empresa", parameters, options);
    }

    private async Task OnEmpresaListChange(EmpresaDto empresaDto)
    {
        _empresas = await EmpresaService.GetActiveEmpresasAsync();
        if (_empresas.Any())
        {
            SelectedEmpresaName = _empresas.Last().Nombre;
        }
        else if (!_empresas.Any())
        {
            SelectedEmpresaName = null;
        }
        else
        {
            SelectedEmpresaName = _empresas.Last().Nombre;
            await Task.FromResult(LoadSelectedEmpresaAsync(SelectedEmpresaId));
        }
    }

    protected override async Task OnInitializedAsync()
    {
        _empresas = await EmpresaService.GetActiveEmpresasAsync();
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Username = await LocalStorage.GetItemAsync<string>("username");
            StateHasChanged();
        }
    }

    private async Task Editar()
    {
        if (IsSelectedEmpresaNameNull)
        {
            Snackbar.Add("Seleccione una empresa primero", Severity.Info);
            return;
        }

        var selectedEmpresa =
            _empresas.SingleOrDefault(e => e.Nombre == SelectedEmpresaName)!.IdEmpresa;
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth         = MaxWidth.Small,
            FullWidth        = true,
        };
        var parameters = new DialogParameters
        {
            {
                "OnEmpresaListChange",
                EventCallback.Factory.Create<EmpresaDto>(this, OnEmpresaListChange)
            },
            {
                "SelectedEmpresaId",
                selectedEmpresa
            }
        };
        await DialogService.ShowAsync<EditarEmpresa>("Edite los datos de la empresa",
            parameters, options);
    }

    private async Task Eliminar()
    {
        if (IsSelectedEmpresaNameNull)
        {
            Snackbar.Add("Seleccione una empresa primero", Severity.Info);
            return;
        }

        var selectedEmpresa =
            _empresas.SingleOrDefault(e => e.Nombre == SelectedEmpresaName)!.IdEmpresa;
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth         = MaxWidth.Small,
            FullWidth        = true,
        };
        var parameters = new DialogParameters
        {
            {
                "SelectedEmpresaId",
                selectedEmpresa
            },
            {
                "OnEmpresaListChange",
                EventCallback.Factory.Create<EmpresaDto>(this, OnEmpresaListChange)
            }
        };
        await DialogService.ShowAsync<EliminarEmpresa>("Edite los datos de la empresa",
            parameters, options);
    }

    private void NavigateToPage()
    {
        if (SelectedEmpresaName is null)
        {
            Snackbar.Add("Seleccione una empresa antes de continuar.", Severity.Info);
            return;
        }

        var selectedEmpresa =
            _empresas.SingleOrDefault(e => e.Nombre == SelectedEmpresaName)!.IdEmpresa;
        var uri = $"/inicio/mainpage/{selectedEmpresa}";
        NavigationManager.NavigateTo(uri);
    }

    private void GenerateReport()
    {
        const string url =
            $"http://localhost:80/Reports/report/Report%20Project1/ListarEmpresas";
        OpenUrlInNewTab(url);
    }


    private void OpenUrlInNewTab(string url)
    {
        var js = $"window.open('{url}', '_blank');";
        JSRuntime.InvokeVoidAsync("eval", js);
    }

    private async Task CerrarSesion()
    {
        IsLoading = true;
        await Task.Delay(2500);
        NavigationManager!.NavigateTo("/");
        IsLoading = false;
    }
}
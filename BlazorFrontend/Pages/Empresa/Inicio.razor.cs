using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Modelos.Models.Dtos;
using BlazorFrontend.Pages.Empresa.Crear;
using BlazorFrontend.Pages.Empresa.Editar;
using BlazorFrontend.Pages.Empresa.Eliminar;
using Microsoft.AspNetCore.Http.Extensions;
using DialogOptions = MudBlazor.DialogOptions;

namespace BlazorFrontend.Pages.Empresa;

public partial class Inicio
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null !;

    private IEnumerable<EmpresaDto> _empresas = new List<EmpresaDto>();
    private int                     SelectedEmpresaId { get; set; }

    public string? SelecteEmpresaName { get; set; }

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
            DisableBackdropClick = true
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
        _empresas = await EmpresaService.GetEmpresasAsync();

        if (empresaDto.IsDeleted)
        {
            SelecteEmpresaName = null;
        }
        else
        {
            SelecteEmpresaName = _empresas.Last().Nombre;
            await Task.FromResult(LoadSelectedEmpresaAsync(SelectedEmpresaId));
        }
    }

    protected override async Task OnInitializedAsync()
    {
        _empresas = await EmpresaService.GetEmpresasAsync();
        StateHasChanged();
    }

    private async Task Editar(MouseEventArgs obj)
    {
        var selectedEmpresa =
            _empresas.SingleOrDefault(e => e.Nombre == SelecteEmpresaName)!.IdEmpresa;
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

    private async void Eliminar(MouseEventArgs obj)
    {
        var selectedEmpresa =
            _empresas.SingleOrDefault(e => e.Nombre == SelecteEmpresaName)!.IdEmpresa;
        if (selectedEmpresa is 0)
        {
            Snackbar.Add("Seleccione una empresa primero", Severity.Info);
        }

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
        var selectedEmpresa =
            _empresas.SingleOrDefault(e => e.Nombre == SelecteEmpresaName)!.IdEmpresa;
        if (selectedEmpresa != default)
        {
            var uri = NavigationManager.ToAbsoluteUri("inicio/mainpage");
            var queryBuilder = new QueryBuilder
            {
                {
                    "id",
                    selectedEmpresa.ToString()
                }
            };
            uri = new UriBuilder(uri)
            {
                Query = queryBuilder.ToString()
            }.Uri;
            NavigationManager.NavigateTo(uri.ToString());
        }
        else
        {
            Snackbar.Add("Seleccione una empresa antes de continuar.", Severity.Info);
        }
    }

    private async Task CerrarSesion()
    {
        await Task.Delay(2500);
        NavigationManager.NavigateTo("/");
    }
}
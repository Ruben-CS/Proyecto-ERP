using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Modelos.Models.Dtos;
using BlazorFrontend.Pages.Empresa.Crear;
using BlazorFrontend.Pages.Empresa.Editar;
using BlazorFrontend.Pages.Empresa.Eliminar;
using Microsoft.AspNetCore.Http.Extensions;
using DialogOptions =  MudBlazor . DialogOptions ;

namespace BlazorFrontend.Pages.Empresa;

public partial class Inicio
{
    [Inject]
    ISnackbar Snackbar { get; set; } = null !;
    private IEnumerable<EmpresaDto> _empresas = new List<EmpresaDto>();
    private int                     SelectedEmpresaId { get; set; }

    private async Task LoadSelectedEmpresaAsync(int selectedId)
    {
        SelectedEmpresaId = selectedId;
        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowMudCrearEmpresaModal()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            MaxWidth         = MaxWidth.Small,
            FullWidth        = true,
            DisableBackdropClick = true
        };
        var parameters = new DialogParameters
        {
            {
                "OnEmpresaAdded",
                EventCallback.Factory.Create<EmpresaDto>(this, OnEmpresaAddedHandler)
            }
        };
        await DialogService.ShowAsync<CrearEmpresa>("Llene los datos de la empresa", parameters, options);
    }

    //todo refactor this duplicate method
    private async Task OnEmpresaAddedHandler(EmpresaDto addedEmpresa)
    {
        _empresas         = await EmpresaService.GetEmpresasAsync();
        SelectedEmpresaId = addedEmpresa.IdEmpresa;
        await LoadSelectedEmpresaAsync(SelectedEmpresaId);
    }

    private async Task OnEmpresaDeletedHandler(EmpresaDto deletedEmpresa)
    {
        _empresas         = await EmpresaService.GetEmpresasAsync();
        SelectedEmpresaId = deletedEmpresa.IdEmpresa;
        await LoadSelectedEmpresaAsync(SelectedEmpresaId);
    }

    protected override async Task OnInitializedAsync()
    {
        _empresas = await EmpresaService.GetEmpresasAsync();
        StateHasChanged();
    }

    private async Task Editar(MouseEventArgs obj)
    {
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
                SelectedEmpresaId
            }
        };
        await DialogService.ShowAsync<EditarEmpresa>("Edite los datos de la empresa", parameters, options);
    }

    private async void Eliminar(MouseEventArgs obj)
    {
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
                SelectedEmpresaId
            },
            {
                "OnEmpresaDeleted",
                EventCallback.Factory.Create<EmpresaDto>(this, OnEmpresaDeletedHandler)
            }
        };
        await DialogService.ShowAsync<EliminarEmpresa>("Edite los datos de la empresa", parameters, options);
    }

    private void NavigateToPage()
    {
        if (SelectedEmpresaId != default)
        {
            var uri = NavigationManager.ToAbsoluteUri("inicio/mainpage");
            var queryBuilder = new QueryBuilder
            {
                {
                    "id",
                    SelectedEmpresaId.ToString()
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
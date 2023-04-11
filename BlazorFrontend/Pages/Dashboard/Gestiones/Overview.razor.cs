using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using BlazorFrontend;
using BlazorFrontend.Shared;
using MudBlazor;
using System.Net.Http.Json;
using Modelos.Models.Dtos;
using global::Services.Gestion;
using Microsoft.AspNetCore.WebUtilities;
using BlazorFrontend.Pages.Gestiones;
using BlazorFrontend.Pages.Gestiones.Editar;
using BlazorFrontend.Pages.Gestiones.Eliminar;
using Modelos.Models.Enums;
using DialogOptions =  MudBlazor . DialogOptions ;
using Size =  MudBlazor . Size ;

namespace BlazorFrontend.Pages.Dashboard.Gestiones
{
    public partial class Overview
    {
        private IEnumerable<GestionDto> _gestiones = new List<GestionDto>().Where(x => x.Estado == EstadosGestion.Abierto);
        [Inject]
        ISnackbar Snackbar { get; set; } = null !;
        [Parameter]
        public int Id { get; set; }

        private readonly DialogOptions _options = new()
        {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            DisableBackdropClick = true
        };
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var uri = new Uri(NavigationManager.Uri);
                var query = QueryHelpers.ParseQuery(uri.Query);
                if (query.TryGetValue("id", out var idValue))
                {
                    Id = int.Parse(idValue!);
                    _gestiones = await GestionServices.GetGestionAsync(Id);
                    StateHasChanged();
                }
                else
                {
                    throw new KeyNotFoundException("The 'id' parameter was not found in the query string.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while initializing the component: {ex}");
            }
        }

        private void ShowMudCrearGestionModal()
        {
            var parameters = new DialogParameters
            {
                {
                    "Id",
                    Id
                },
            };
            DialogService.ShowAsync<CrearGestion>("Llene los datos de la gestion", parameters, _options);
        }

        private void EditarGestion(GestionDto item)
        {
            var parameters = new DialogParameters
            {
                {
                    "Id",
                    item.IdGestion
                },
                {
                    "GestionDto",
                    item
                }
            };
            DialogService.ShowAsync<EditarGestion>("Editar gestion", parameters, _options);
        }

        private async Task BorrarGestion(GestionDto item)
        {
            var parameters = new DialogParameters
            {
                {
                    "Id",
                    item.IdGestion
                }
            };
            await DialogService.ShowAsync<EliminarGestion>("Editar gestion", parameters, _options);
        }

        private void NavigateToPage(GestionDto gestion)
        {
            if (gestion.IdGestion is not 0)
            {
                var uri = $"/overview/inicioperiodo/{gestion.IdGestion}";
                NavigationManager.NavigateTo(uri);
            }
            else
            {
                Snackbar.Add("Seleccione una empresa antes de continuar.", Severity.Info);
            }
        }
    }
}
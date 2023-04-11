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
using Modelos.Models.Enums;
using ButtonType =  MudBlazor . ButtonType ;

namespace BlazorFrontend.Pages.Gestiones
{
    public partial class CrearGestion
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }

        [Parameter]
        public int Id { get; set; }

        public GestionDto GestionDto { get; } = new();
        private IEnumerable<GestionDto> _gestionDtos = new List<GestionDto>();
        protected override async Task OnInitializedAsync()
        {
            _gestionDtos = await GestionServices.GetGestionAsync(Id);
            StateHasChanged();
        }

        private async Task ValidateAndCreateGestion()
        {
            var url = $"https://localhost:44378/gestiones/agregarGestion/{Id}";
            var gestionDto = new GestionDto
            {
                Nombre = GestionDto.Nombre,
                FechaInicio = GestionDto.FechaInicio,
                FechaFin = GestionDto.FechaFin,
                IdEmpresa = Id
            };
            if (!await ValidateNumberOfActiveGestiones())
            {
                Snackbar.Add("Ya existen dos gestiones activas", Severity.Error);
            }
            else if (!await ValidateUniqueNombre())
            {
                Snackbar.Add("Ya existe una gesti�n con ese nombre", Severity.Error);
            }
            else if (!await ValidateFechaInicioAndFechaFin())
            {
                Snackbar.Add("Las fechas no son v�lidas", Severity.Error);
            }
            else if (await FechasNoSolapadan())
            {
                Snackbar.Add("Las fechas solapan!", Severity.Error);
            }
            else
            {
                var response = await HttpClient.PostAsJsonAsync(url, gestionDto);
                Snackbar.Add("Gesti�n creada exitosamente", Severity.Success);
                MudDialog!.Close(DialogResult.Ok(response));
            }
        }

        private async Task<bool> ValidateNumberOfActiveGestiones()
        {
            return await Task.FromResult(_gestionDtos.Count(gestion => gestion.IdEmpresa == Id && gestion.Estado == EstadosGestion.Abierto) < 2);
        }

        private async Task<bool> ValidateUniqueNombre()
        {
            return await Task.FromResult(!_gestionDtos.Any(gestion => gestion.Nombre == GestionDto.Nombre && gestion.IdEmpresa == Id));
        }

        private async Task<bool> ValidateFechaInicioAndFechaFin()
        {
            return await Task.FromResult(GestionDto.FechaInicio < GestionDto.FechaFin);
        }

        public async Task<bool> FechasNoSolapadan()
        {
            var gestionActiva = _gestionDtos.Single(gestion => gestion.IdEmpresa == Id && gestion.Estado == EstadosGestion.Abierto);
            if (GestionDto.FechaInicio >= gestionActiva.FechaInicio && GestionDto.FechaInicio <= gestionActiva.FechaFin || GestionDto.FechaFin >= gestionActiva.FechaInicio && GestionDto.FechaFin <= gestionActiva.FechaFin)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        void Cancel() => MudDialog!.Cancel();
    }
}
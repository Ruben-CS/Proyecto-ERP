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
using global::Services.Periodo;
using global::Services.Gestion;
using Microsoft.IdentityModel.Tokens;
using Modelos.Models.Enums;

namespace BlazorFrontend.Pages.Periodo.Crear
{
    public partial class CrearPeriodo
    {
        [CascadingParameter]
        MudDialogInstance? MudDialog { get; set; }

        [Parameter]
        public int IdGestion { get; set; }

        [Parameter]
        public int IdEmpresa { get; set; }

        private IEnumerable<PeriodoDto> _periodoDtos = new List<PeriodoDto>();
        private IEnumerable<GestionDto> _gestionDtos = new List<GestionDto>();
        public PeriodoDto PeriodoDto { get; set; } = new();
        void Cancel() => MudDialog!.Cancel();
        protected override async Task OnInitializedAsync()
        {
            _periodoDtos = await PeriodoService.GetPeriodosAsync(IdGestion);
            _gestionDtos = await GestionServices.GetGestionAsync(IdEmpresa);
            StateHasChanged();
        }

        private async Task ValidateAndCreatePeriodo()
        {
            var url = $"https://localhost:44378/periodos/crearperiodo/{IdGestion}";
            var periodoDto = new PeriodoDto
            {
                Nombre = PeriodoDto.Nombre,
                FechaInicio = PeriodoDto.FechaInicio,
                FechaFin = PeriodoDto.FechaFin,
                IdGestion = IdGestion
            };
            if (!await ValidateUniqueNombre())
            {
                Snackbar.Add("Ya existe un periodo con ese nombre", Severity.Error);
            }
            else if (await FechaInicioEsMayor())
            {
                Snackbar.Add("La fecha de inicio no puede ser mayor a la final", Severity.Error);
            }
            else if (await FechasSonIguales())
            {
                Snackbar.Add("Las fechas no pueden ser iguales", Severity.Error);
            }
            else if (await FechasDentroDelRangoDeGestion())
            {
                Snackbar.Add("La fecha debe estar dentro del rango de la gestion", Severity.Error);
            }
            else if (await FechasNoSolapan())
            {
                Snackbar.Add("Las fechas solapan con un periodo existente", Severity.Error);
            }
            else
            {
                var response = await HttpClient.PostAsJsonAsync(url, periodoDto);
                Snackbar.Add("Periodo creado exitosamente", Severity.Success);
                MudDialog!.Close(DialogResult.Ok(response));
            }
        }

        private async Task<bool> ValidateUniqueNombre()
        {
            return await Task.FromResult(!_periodoDtos.Any(periodo => periodo.Nombre == PeriodoDto.Nombre && periodo.IdGestion == IdGestion));
        }

        private async Task<bool> FechasDentroDelRangoDeGestion()
        {
            var gestion = _gestionDtos.FirstOrDefault(gestion => gestion.IdGestion == IdGestion);
            if (gestion is null)
            {
                throw new NullReferenceException("gestion no existe");
            }

            if (PeriodoDto.FechaInicio < gestion.FechaInicio || PeriodoDto.FechaInicio > gestion.FechaFin || PeriodoDto.FechaFin < gestion.FechaInicio || PeriodoDto.FechaFin > gestion.FechaFin)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        private async Task<bool> FechasSonIguales()
        {
            return await Task.FromResult(PeriodoDto.FechaInicio == PeriodoDto.FechaFin);
        }

        private async Task<bool> FechaInicioEsMayor()
        {
            return await Task.FromResult(PeriodoDto.FechaInicio > PeriodoDto.FechaFin);
        }

        private async Task<bool> FechasNoSolapan()
        {
            var periodoActivo = _periodoDtos.Where(periodo => periodo.IdGestion == IdGestion && periodo.Estado == EstadosPeriodo.Abierto).ToList();
            if (periodoActivo.IsNullOrEmpty())
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(periodoActivo.Any(periodo => PeriodoDto.FechaInicio >= periodo.FechaInicio && PeriodoDto.FechaInicio <= periodo.FechaFin || PeriodoDto.FechaFin >= periodo.FechaInicio && PeriodoDto.FechaFin <= periodo.FechaFin));
        }
    }
}
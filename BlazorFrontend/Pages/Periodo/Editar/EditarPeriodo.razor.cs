using Microsoft.AspNetCore.Components;
using MudBlazor;
using Modelos.Models.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace BlazorFrontend.Pages.Periodo.Editar
{
    public partial class EditarPeriodo
    {
        [CascadingParameter]
        private MudDialogInstance? MudDialog { get; set; }

        [Parameter]
        public PeriodoDto PeriodoDto { get; set; } = null!;

        [Parameter]
        public int IdGestion { get; set; }

        [Parameter]
        public int IdPeriodo { get; set; }

        private void                    Cancel() => MudDialog!.Cancel();
        private IEnumerable<PeriodoDto> _periodoDtos = new List<PeriodoDto>();
        private async Task ValidateAndEditPeriodo()
        {
            var url = $"https://localhost:44378/periodos/actualizarperiodo/{IdGestion}/{IdPeriodo}";
            var editedPeriodo = new PeriodoDto
            {
                IdPeriodo = IdPeriodo,
                Nombre = PeriodoDto.Nombre,
                FechaInicio = PeriodoDto.FechaInicio,
                FechaFin = PeriodoDto.FechaFin,
                IdGestion = IdGestion
            };
            if (await ValidateUniqueNombre())
            {
                Snackbar.Add("Ya existe un periodo con ese nombre", Severity.Error);
            }
            else if (await FechasNoSolapan())
            {
                Snackbar.Add("La fecha solapa con un periodo existente", Severity.Error);
            }
            else
            {
                var response = await HttpClient.PutAsJsonAsync(url, editedPeriodo);
                Snackbar.Add("Empresa editata correctamente", Severity.Success);
                MudDialog!.Close(DialogResult.Ok(response));
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _periodoDtos = await PeriodoService.GetPeriodosAsync(IdGestion);
            StateHasChanged();
        }

        private async Task<bool> ValidateUniqueNombre()
        {
            if (_periodoDtos.Any(periodo => periodo.Nombre == PeriodoDto.Nombre))
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        private async Task<bool> FechasNoSolapan()
        {
            var periodoActivo = _periodoDtos.Where(periodo => periodo.IdGestion == IdGestion && periodo.IdPeriodo != IdPeriodo).ToList();
            if (periodoActivo.IsNullOrEmpty())
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(periodoActivo.Any(periodo => PeriodoDto.FechaInicio >= periodo.FechaInicio && PeriodoDto.FechaInicio <= periodo.FechaFin || PeriodoDto.FechaFin >= periodo.FechaInicio && PeriodoDto.FechaFin <= periodo.FechaFin));
        }
    }
}
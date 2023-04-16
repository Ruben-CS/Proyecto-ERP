using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.Gestiones.Editar;
public partial class EditarGestion
{
     [CascadingParameter]
     private MudDialogInstance? MudDialog { get; set; }

    private readonly bool _click = false;
    private          bool _focus = false;

    [Parameter]
    public int Id { get; set; }

    [Parameter]
    public GestionDto GestionDto { get; set; } = null!;

    [Parameter]
    public EventCallback<GestionDto> OnDataGridChange { get; set; }

    private IEnumerable<GestionDto> _gestionDtos = new List<GestionDto>();

    private IEnumerable<PeriodoDto> _periodosPorGestion = new List<PeriodoDto>();

    protected override async Task OnInitializedAsync()
    {
        _gestionDtos = await GestionServices.GetGestionAsync(GestionDto.IdEmpresa);
        _periodosPorGestion = await PeriodoService.GetPeriodosAsync(Id);
        await base.OnInitializedAsync();
    }


    private async Task ValidateAndEditGestion()
    {
        var url = $"https://localhost:44378/gestiones/actualizarGestion/{Id}";

        var editedGestion = new GestionDto
        {
            IdGestion = Id,
            Nombre = GestionDto.Nombre,
            FechaInicio = GestionDto.FechaInicio,
            FechaFin = GestionDto.FechaFin,
            IdEmpresa = GestionDto.IdEmpresa
        };
        if (ValidateEmptyPeriodoInGestion())
        {
            if (await ValidateUniqueNombre(editedGestion))
            {
                Snackbar.Add("Ya existe una gestion con ese nombre", Severity.Error);
                return;
            }
            await Edit(editedGestion, url);
            return;
        }
        if (await ValidateFechaInicioAndFechaFin())
        {
            Snackbar.Add("La fecha de inicio no puede ser mayor a la fecha final",
                Severity.Error);
            return;
        }
        if (await ValidateUniqueNombre(editedGestion))
        {
            Snackbar.Add("Ya existe una gestion con ese nombre", Severity.Error);
            return;
        }
        if (await ValidateEqualDates())
        {
            Snackbar.Add("Las fechas no pueden ser iguales", Severity.Error);
            return;
        }
        if (await FechasNoSolapadan(editedGestion))
        {
            Snackbar.Add("Las fechas solapan con una gestion activa", Severity.Error);
            return;
        }
        if (await ValidateClosedGestion(editedGestion))
        {
            Snackbar.Add("No puede editar una gestion cerrada", Severity.Error);
            return;
        }
        await Edit(editedGestion, url);
    }

    private async Task Edit(GestionDto editedGestion, string url)
    {
        var response = await HttpClient.PutAsJsonAsync(url, editedGestion);
        Snackbar.Add("Empresa editata correctamente", Severity.Success);
        await OnDataGridChange.InvokeAsync(GestionDto);
        MudDialog!.Close(DialogResult.Ok(response));
    }

    //todo fix the null check
    private async Task<bool> FechasNoSolapadan(GestionDto gestionDto)
    {
        var gestionActiva = _gestionDtos.SingleOrDefault(gestion =>
            gestion.IdEmpresa == gestionDto.IdEmpresa
            && gestion.Estado == EstadosGestion.Abierto
            && gestion.IdGestion != gestionDto.IdGestion
            );

        if (gestionActiva is null)
        {
            return await Task.FromResult(false);
        }

        if (
            gestionDto.FechaInicio >= gestionActiva.FechaInicio &&
            gestionDto.FechaInicio <= gestionActiva.FechaFin ||
            gestionDto.FechaFin >= gestionActiva.FechaInicio &&
            gestionDto.FechaFin <= gestionActiva.FechaFin)
        {
            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }

    private static async Task<bool> ValidateClosedGestion(GestionDto gestionDto)
    {
        return await Task.FromResult(gestionDto.Estado is EstadosGestion.Cerrado);
    }

    private bool ValidateEmptyPeriodoInGestion()
    {
        var gestionConPeriodo = _periodosPorGestion.Where(periodo =>
            periodo.IdGestion == Id).ToList();
        return gestionConPeriodo.Count > 0;
    }

    private async Task<bool> ValidateUniqueNombre(GestionDto gestionDto)
    {
        return await Task.FromResult(_gestionDtos.Any(gestion =>
            string.Equals(gestion.Nombre, gestionDto.Nombre,
                StringComparison.OrdinalIgnoreCase) &&
            gestion.IdGestion != gestionDto.IdGestion &&
            gestion.IdEmpresa == gestionDto.IdEmpresa
            ));
    }

    private async Task<bool> ValidateFechaInicioAndFechaFin() =>
        await Task.FromResult(GestionDto.FechaInicio > GestionDto.FechaFin);

    private async Task<bool> ValidateEqualDates() =>
        await Task.FromResult(GestionDto.FechaInicio == GestionDto.FechaFin);

    void Cancel() => MudDialog!.Cancel();
}
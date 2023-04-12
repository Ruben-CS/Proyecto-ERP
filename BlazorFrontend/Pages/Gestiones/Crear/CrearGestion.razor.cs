using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.Gestiones.Crear;

public partial class CrearGestion
{
    [CascadingParameter]
    MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public int Id { get; set; }

    public  GestionDto              GestionDto { get; } = new();
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
            Nombre      = GestionDto.Nombre,
            FechaInicio = GestionDto.FechaInicio,
            FechaFin    = GestionDto.FechaFin,
            IdEmpresa   = Id
        };
        if (!await ValidateNumberOfActiveGestiones())
        {
            Snackbar.Add("Ya existen dos gestiones activas", Severity.Error);
        }
        else if (!await ValidateUniqueNombre())
        {
            Snackbar.Add("Ya existe una gestión con ese nombre", Severity.Error);
        }
        else if (await ValidateFechaInicioAndFechaFin())
        {
            Snackbar.Add("La fecha inicio no puede ser mayor a la fecha final",
                Severity.Error);
        }
        else if (await FechasNoSolapadan())
        {
            Snackbar.Add("Las fechas solapan!", Severity.Error);
        }else if (await ValidateEqualDates())
        {
            Snackbar.Add("Las fechas no pueden ser iguales", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, gestionDto);
            Snackbar.Add("Gestión creada exitosamente", Severity.Success);
            MudDialog!.Close(DialogResult.Ok(response));
            StateHasChanged();
        }
    }

    private async Task<bool> ValidateNumberOfActiveGestiones() =>
        await Task.FromResult(_gestionDtos.Count(gestion =>
            gestion.IdEmpresa == GestionDto.IdEmpresa
            & gestion.Estado  == EstadosGestion.Abierto) < 2);

    private async Task<bool> ValidateUniqueNombre() =>
        await Task.FromResult(!_gestionDtos.Any(gestion =>
            gestion.Nombre       == GestionDto.Nombre
            && gestion.IdEmpresa == GestionDto.IdEmpresa
            && gestion.Estado    == EstadosGestion.Abierto
        ));

    private async Task<bool> ValidateFechaInicioAndFechaFin() =>
        await Task.FromResult(GestionDto.FechaInicio > GestionDto.FechaFin);

    private async Task<bool> ValidateEqualDates() =>
        await Task.FromResult(GestionDto.FechaInicio == GestionDto.FechaFin);

    public async Task<bool> FechasNoSolapadan()
    {
        var gestionActiva = _gestionDtos.SingleOrDefault(gestion =>
            gestion.IdEmpresa == Id && gestion.Estado == EstadosGestion.Abierto);
        if (gestionActiva is null)
        {
            return await Task.FromResult(false);
        }

        if (GestionDto.FechaInicio >= gestionActiva.FechaInicio &&
            GestionDto.FechaInicio <= gestionActiva.FechaFin ||
            GestionDto.FechaFin >= gestionActiva.FechaInicio &&
            GestionDto.FechaFin <= gestionActiva.FechaFin)
        {
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }

    private void Cancel() => MudDialog!.Cancel();
}
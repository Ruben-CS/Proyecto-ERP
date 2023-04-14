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

    [Parameter]
    public EventCallback<GestionDto> OnGestionAdded { get; set; }

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
        if (await ValidateNumberOfActiveGestiones(gestionDto))
        {
            Snackbar.Add("Ya existen dos gestiones activas", Severity.Error);
        }
        else if (await ValidateUniqueNombre(gestionDto))
        {
            Snackbar.Add("Ya existe una gestion con ese nombre", Severity.Error);
        }
        else if (await ValidateFechaInicioAndFechaFin())
        {
            Snackbar.Add("La fecha inicio no puede ser mayor a la fecha final",
                Severity.Error);
        }
        else if (await FechasNoSolapadan())
        {
            Snackbar.Add("Las fechas solapan con una gestion activa", Severity.Error);
        }
        else if (await ValidateEqualDates())
        {
            Snackbar.Add("Las fechas no pueden ser iguales", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, gestionDto);
            Snackbar.Add("Gestion creada exitosamente", Severity.Success);
            await OnGestionAdded.InvokeAsync(gestionDto);
            MudDialog!.Close(DialogResult.Ok(response));
        }
    }

    private async Task<bool> ValidateNumberOfActiveGestiones(GestionDto gestionDto)
    {
        return await Task.FromResult(_gestionDtos.Count(gestion =>
            gestion.IdEmpresa == gestionDto.IdEmpresa
            && gestion.Estado == EstadosGestion.Abierto) == 2);
    }


    private async Task<bool> ValidateUniqueNombre(GestionDto gestionDto) =>
        await Task.FromResult(_gestionDtos.Any(gestion =>
            gestion.Nombre       == gestionDto.Nombre
            && gestion.IdEmpresa == gestionDto.IdEmpresa
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
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Empresa.Crear;

public partial class CrearEmpresa
{
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Parameter]
    public EventCallback<EmpresaDto> OnEmpresaListChange { get; set; }

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    private List<MonedaDto> _monedas = new();

    private          MonedaDto MonedaDto { get; set; } = new();
    private          EmpresaDto EmpresaDto { get; } = new();
    private readonly List<int> _listaNiveles = Enumerable.Range(3, 5).ToList();
    private          IEnumerable<EmpresaDto> _empresaDtos = new List<EmpresaDto>();
    private          void Cancel() => MudDialog!.Cancel();

    protected override async Task OnInitializedAsync()
    {
        _empresaDtos = await EmpresaService.GetEmpresasAsync();
        _monedas     = (await MonedaService.GetMonedasAsync())!;
    }

    public  string? SelectedMoneda { get; set; }
    private string? _previousSelectedMoneda;

    protected override void OnParametersSet()
    {
        if (SelectedMoneda == _previousSelectedMoneda) return;
        _previousSelectedMoneda = SelectedMoneda;
        SelectedMoneda = _monedas.FirstOrDefault(m => m.Nombre == SelectedMoneda)?.Nombre;
        MonedaDto = _monedas.Single(m => m.Nombre == SelectedMoneda);
    }

    private async Task UpsertEmpresa()
    {
        const string url = "https://localhost:44378/empresas/agregarempresa";

        var empresaDto = new EmpresaDto
        {
            Nombre    = EmpresaDto.Nombre,
            Nit       = EmpresaDto.Nit,
            Sigla     = EmpresaDto.Sigla,
            Telefono  = EmpresaDto.Telefono,
            Correo    = EmpresaDto.Correo,
            Direccion = EmpresaDto.Direccion,
            Niveles   = EmpresaDto.Niveles,
            IdUsuario = 1
        };

        if (!await ValidateUniqueNombre())
        {
            Snackbar.Add("Existe una empresa con ese nombre", Severity.Error);
        }
        else if (!await ValidateUniqueNit())
        {
            Snackbar.Add("Existe una empresa con el mismo NIT", Severity.Error);
        }
        else if (!await ValidateUniqueSigla())
        {
            Snackbar.Add("Existe una empresa con esas siglas", Severity.Error);
        }
        else if (await ValidateEmptyEmpresa())
        {
            Snackbar.Add("Rellene los datos esenciales", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, empresaDto);
            Snackbar.Add("Empresa creada exitosamente", Severity.Success);
            var addedEmpresa = await response.Content.ReadFromJsonAsync<EmpresaDto>();
            await OnEmpresaListChange.InvokeAsync(addedEmpresa);
            await CreateEmpresaMoneda();
            await CreateDefaultCuentas();
            MudDialog!.Close(DialogResult.Ok(response));
        }
    }

    private async Task CreateEmpresaMoneda()
    {
        var empresas   = await EmpresaService.GetEmpresasAsync();
        var newEmpresa = empresas.Last();
        MonedaDto = _monedas.Single(m => m.Nombre == SelectedMoneda);
        var url =
            $"https://localhost:44352/empresaMonedas/agregarempresamoneda/{newEmpresa.IdEmpresa}/{MonedaDto.IdMoneda}";

        var empresaMonedaDto = new EmpresaMonedaDto
        {
            Cambio              = null,
            IdEmpresa           = newEmpresa.IdEmpresa,
            IdMonedaPrincipal   = MonedaDto.IdMoneda,
            IdMonedaAlternativa = null,
            IdUsuario           = 1
        };
        await HttpClient.PostAsJsonAsync(url, empresaMonedaDto);
    }

    private async Task CreateDefaultCuentas()
    {
        var lasEmpresaCreated    = await EmpresaService.GetEmpresasAsync();
        var lastEmpresaCreatedId = lasEmpresaCreated.Last().IdEmpresa;
        var url =
            $"https://localhost:44378/cuentas/CrearCuentasPorDefecto/{lastEmpresaCreatedId}";
       var response = await HttpClient.PostAsJsonAsync(url, lastEmpresaCreatedId);
       if (response.IsSuccessStatusCode)
       {
           Snackbar.Add("Cuentas creada", Severity.Info);
       }
       else
       {
           Snackbar.Add("Error al crear las cuentas", Severity.Error);
       }
    }

    private async Task<bool> ValidateUniqueNombre() =>
        await Task.FromResult(!_empresaDtos.Any(empresa =>
            empresa.Nombre    == EmpresaDto.Nombre &&
            empresa.IsDeleted == false
        ));

    private async Task<bool> ValidateUniqueNit() =>
        await Task.FromResult(!_empresaDtos.Any(empresa =>
            empresa.Nit       == EmpresaDto.Nit &&
            empresa.IsDeleted == false
        ));

    private async Task<bool> ValidateUniqueSigla() =>
        await Task.FromResult(!_empresaDtos.Any(empresa =>
            empresa.Sigla     == EmpresaDto.Sigla &&
            empresa.IsDeleted == false
        ));

    private async Task<bool> ValidateEmptyEmpresa() =>
        await Task.FromResult(string.IsNullOrEmpty(EmpresaDto.Nombre) ||
                              string.IsNullOrEmpty(EmpresaDto.Nit)    ||
                              string.IsNullOrEmpty(EmpresaDto.Sigla));
}
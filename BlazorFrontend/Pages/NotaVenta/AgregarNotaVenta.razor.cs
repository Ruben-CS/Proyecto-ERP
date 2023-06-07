using System.Collections.ObjectModel;
using System.Text;
using BlazorFrontend.Pages.Nota.AgregarNota;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.NotaVenta;

public partial class AgregarNotaVenta
{
    private bool     _success;
    private MudForm? _form;

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Nota Details

    private string? NroNota { get; set; }

    private string? Descripcion { get; set; }

    private DateTime? Fecha { get; set; } = DateTime.Today;

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; } = new();

    #endregion

    private List<NotaDto> Notas { get; set; } = new();

    private List<ArticuloDto> Articulos { get; set; } = new();

    private readonly ObservableCollection<DetalleDto> _detalleParaVenta = new();

    private async Task GoBack() =>
        await Task.FromResult(JSRuntimeExtensions.InvokeVoidAsync(JsRuntime, "blazorBrowserHistory.goBack"));

    private void AddNewDetalleLote(DetalleDto lote) => _detalleParaVenta.Add(lote);


    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        await GetNotaCompras();
        var nextNro = GetNextNumeroNota();
        NroNota = nextNro.ToString();
        await InvokeAsync(StateHasChanged);
    }

    private int GetNextNumeroNota()
    {
        var maxNroNota = Notas.Max(n => n.NroNota);
        return (maxNroNota ?? 0) + 1;
    }

    private async Task OpenAgregarDetalle()
    {
        var parameters = new DialogParameters
        {
            { "FechaIngreso", Fecha },
            { "Articulos", Articulos },
            { "IdEmpresa", IdEmpresa },
            {
                "AddNewDetalleLote",
                EventCallback.Factory.Create<DetalleDto>(this, AddNewDetalleLote)
            },
            {"_detalleParaLote", _detalleParaVenta}
        };

        await DialogService.ShowAsync<AgregarDetalleModal>("Ingrese los detalles",
            parameters, _options);
    }


    private async Task AgregarVenta()
    {
        if (_detalleParaVenta.Count == 0)
        {
            Snackbar.Add("Debe agregar al menos un articulo", Severity.Info);
            return;
        }

        var          total   = _detalleParaVenta.Sum(d => d.PrecioVenta * d.Cantidad);
        const string url     = "https://localhost:44321/notas/agregarNota";
        var          nroNota = GetNextNumeroNota();
        var nota = new NotaDto
        {
            NroNota       = nroNota,
            Fecha         = Fecha!.Value,
            Descripcion   = Descripcion!,
            Total         = total,
            IdEmpresa     = IdEmpresa,
            IdUsuario     = 1,
            IdComprobante = null,
            TipoNota      = TipoNota.Venta
        };
        var response = await HttpClient.PostAsJsonAsync(url, nota);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Nota agregada exitosamente", Severity.Success);
            await AgregarLote();
            NavigationManager.NavigateTo(
                $"/anularNotaCompra/{IdEmpresa}/{Notas.Last().IdNota}");
        }
    }

    private async Task GetNotaCompras() =>
        Notas = (await NotaService.GetNotaVentasAsync(IdEmpresa))!;

    private async Task AgregarLote()
    {
        await GetNotaCompras();
        var ultimoIdNota = Notas.Last().IdNota;
        var url          = $"https://localhost:44321/lotes/agregarLote/{ultimoIdNota}";
        var emptyContent = new StringContent("", Encoding.UTF8, "application/json");
        try
        {
            foreach (var lote in _detalleParaVenta)
            {
                lote.IdNota = ultimoIdNota;
                var nroLote = await CheckIfLoteHasExistingArticulo();
                lote.NroLote = nroLote!.Value;
                var urlEditarCantidadArticulo =
                    $"https://localhost:44321/articulos/editarArticuloCantidad/{lote.IdArticulo}/{lote.Cantidad}";
                var response = await HttpClient.PostAsJsonAsync(url, lote);
                var responseEditar =
                    await HttpClient.PutAsJsonAsync(urlEditarCantidadArticulo,
                        emptyContent);
                response.EnsureSuccessStatusCode();
                responseEditar.EnsureSuccessStatusCode();
            }

            Snackbar.Add("Detalles guardados exitosamente", Severity.Success);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while posting details: {e.Message}");
            Snackbar.Add("Error al guardar los detalles", Severity.Error);
        }
    }

    private async Task<int?> CheckIfLoteHasExistingArticulo()
    {
        var lotePorArticulo = await NotaService.GetNotaVentasAsync(IdEmpresa);
        if (lotePorArticulo.IsNullOrEmpty())
            return 1;
        return lotePorArticulo!.Max(n => n.NroNota) + 1;
    }
}
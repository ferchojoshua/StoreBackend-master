using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.ReportHelper
{
    public interface IReportsHelper
    {
        Task<ICollection<Sales>> ReportMasterVentas(MasterVentasViewModel model);
        Task<ICollection<Abono>> ReportIngresos(IngresosViewModel model);
        Task<bool> GenerarHistoricoDocumentosPorCobrarAsync();
        Task<ICollection<ProductIn>> ReportCompras(ComprasViewModel model);
        Task<ICollection<TrasladoResponse>> ReportproductMovments(TrasladoInventarioViewModel model);
        Task<ICollection<Sales>> ReportCuentasXCobrar(CuentasXCobrarViewModel model);
        Task<ICollection<HistoricalReceivablesDocuments>> RHistoricalReceivablesDocuments(CuentasXCobrarViewModel model);
        Task<ICollection<ReportResponse>> ReportArticulosVendidos(ArtVendidosViewModel model);
        Task<ICollection<Existence>> ReportArticulosNoVendidos(ArtNoVendidosViewModel model);
        Task<ICollection<CajaMovment>> ReportCajaChica(CajaChicaViewModel model);
        Task<DailyCloseResponse> ReportCierreDiario(CierreDiarioViewModel model);
        Task<IEnumerable<ProductosInventario>> GetProductosInventarioAsync(int? productID, int? storeID, int? tipoNegocioID, int? familiaID, bool? showststore, bool? OmitirStock);
    }
}

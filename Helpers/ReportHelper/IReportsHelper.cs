using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.ReportHelper
{
    public interface IReportsHelper
    {
        Task<ICollection<Sales>> ReportMasterVentas(MasterVentasViewModel model);
        Task<ICollection<Sales>> ReportCuentasXCobrar(CuentasXCobrarViewModel model);
        Task<ICollection<ReportResponse>> ReportArticulosVendidos(ArtVendidosViewModel model);
        Task<ICollection<Sales>> ReportCierreDiario(CierreDiarioViewModel model);
    }
}

using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.FacturacionHelper
{
    public interface IFacturationHelper
    {
        Task<Facturacion> AddFacturacionAsync(AddFacturacionViewModel model, Entities.User user);

        //Task<ICollection<Facturacion>> GetProformasFacturacionAsync();
       
        
        //Task<Facturacion> UpdateFacturacionAsync(AddupdateViewModel model, Entities.User user);
        Task<Sales> PayFacturaAsync(PayFactViewModel model, Entities.User user);
        Task<ICollection<Facturacion>> GetFacturacionAsync(int storeId);
        Task<ICollection<TipoPago>> GetTipoPagoAsync();
        Task<ICollection<Facturacion>> GetCancelledFacturacionAsync(int storeId);
        Task<Sales> GetReprintBillAsync(int saleId);
        Task<ICollection<Facturacion>> GetAnulatedFacturacionAsync(int storeId);
        Task<Facturacion> DeleteFacturacionAsync(int factId, Entities.User user);
    }
}

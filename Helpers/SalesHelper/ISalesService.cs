using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public interface ISalesService
    {
        Task<ICollection<Sales>> GetContadoSalesByStoreAsync(int idStore);
        Task<ICollection<Sales>> GetCreditoSalesByStoreAsync(int idStore);
        Task<ICollection<Sales>> GetAnulatedSalesByStoreAsync(int idStore);

        Task<ICollection<Abono>> GetQuoteListAsync(int id);
        Task<ICollection<GetSalesAndQuotesResponse>> GetSalesUncanceledByClientAsync(int idClient);

        Task<Sales> AnularSaleAsync(int id, Entities.User user);
        Task<Sales> AnularSaleParcialAsync(EditSaleViewModel model, Entities.User user);
        Task<Sales> AddSaleAsync(AddSaleViewModel model, Entities.User user);
        Task<ICollection<Abono>> AddAbonoAsync(AddAbonoViewModel model, Entities.User user);
        Task<Abono> AddAbonoEspecificoAsync(AddAbonoEspecificoViewModel model, Entities.User user);
    }
}

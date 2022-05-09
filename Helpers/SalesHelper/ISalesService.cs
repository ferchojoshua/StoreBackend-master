using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public interface ISalesService
    {
        Task<ICollection<Sales>> GetSalesListAsync();

        Task<ICollection<Abono>> GetQuoteListAsync(int id);

        Task<Sales> AnularSaleAsync(int id, Entities.User user);
        Task<Sales> AnularSaleParcialAsync(EditSaleViewModel model, Entities.User user);
        Task<Sales> AddSaleAsync(AddSaleViewModel model, Entities.User user);
        Task<Abono> AddAbonoAsync(AddAbonoViewModel model, Entities.User user);
    }
}

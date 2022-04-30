using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public interface ISalesService
    {
        Task<ICollection<Sales>> GetSalesListAsync();

        // Task<Client> GetClientAsync(int id);
        Task<Sales> AddSaleAsync(AddSaleViewModel model, Entities.User user);
        // Task<Client> UpdateClientAsync(UpdateClientViewModel model, Entities.User user);
        // Task<Client> DeleteClientAsync(int id);
    }
}

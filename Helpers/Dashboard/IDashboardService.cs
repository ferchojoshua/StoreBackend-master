using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.ClientService
{
    public interface IDashboardService
    {
        Task<ICollection<GetSalesByDateResponse>> GetSalesByDateAsync(int idStore);
        Task<decimal> GetSalesMonthByStoreAsync(int idStore);
        Task<ICollection<decimal>> GetSalesRecupMonthAsync(int idStore);
        Task<decimal> GetSalesWeekByStoreAsync(int idStore);
        Task<int> GetNewClientsByStoreAsync(int idStore);
        // Task<Client> GetClientAsync(int id);
        // Task<Client> AddClientAsync(AddClientViewModel model, Entities.User user);
        // Task<Client> UpdateClientAsync(UpdateClientViewModel model, Entities.User user);
        // Task<Client> DeleteClientAsync(int id);
    }
}

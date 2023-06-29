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
        Task<ICollection<int>> GetVisitedClientsByStoreAsync(int idStore);
        Task<ICollection<Client>> GetClientsByLocationAndStoreAsync(int idStore);
        Task<ICollection<Sales>> GetSalesByTNAndStoreAsync(int idStore);
    }
}

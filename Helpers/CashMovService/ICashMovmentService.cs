using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public interface ICashMovmentService
    {
        Task<ICollection<CajaMovment>> GetCashMovmentByStoreAsync(int idStore);
        Task<CajaMovment> AddCashMovmentAsync(AddCashMovmentViewModel model, Entities.User user);
    }
}

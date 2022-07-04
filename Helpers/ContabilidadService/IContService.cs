using Store.Entities;
using StoreBackend.Models.ViewModels;

namespace StoreBackend.Helpers.ContabilidadService
{
    public interface IContService
    {
        Task<ICollection<Count>> GetCountListAsync();
        Task<ICollection<CountGroup>> GetCountGroupsAsync();
        Task<Count> AddCountAsync(AddCountViewModel model);
        Task<Count> DeleteCountAsync(int idCuenta);
        Task<Count> UpdateCountAsync(UpdateCountViewModel model);
    }
}

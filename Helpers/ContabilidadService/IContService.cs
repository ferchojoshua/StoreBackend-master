using Store.Entities;
using Store.Models.ViewModels;
using StoreBackend.Models.ViewModels;

namespace StoreBackend.Helpers.ContabilidadService
{
    public interface IContService
    {
        Task<ICollection<Count>> GetCountListAsync();
        Task<ICollection<CountLibros>> GetCountLibrosAsync();
        Task<ICollection<CountFuentesContables>> GetCountFuentesContablesAsync();
        Task<ICollection<CountGroup>> GetCountGroupsAsync();
        Task<Count> AddCountAsync(AddCountViewModel model);
        Task<Count> DeleteCountAsync(int idCuenta);
        Task<Count> UpdateCountAsync(UpdateCountViewModel model);

        //Rerports
        Task<ICollection<ExistencesDailyCheck>> GetExistencesReportAsync(
            ProdHistoryViewModel model
        );
    }
}

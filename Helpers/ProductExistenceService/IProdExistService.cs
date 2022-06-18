using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.ProductExistenceService
{
    public interface IProdExistService
    {
        Task<ICollection<ExistenciaResponse>> GetProductExistencesAsync();
    }
}

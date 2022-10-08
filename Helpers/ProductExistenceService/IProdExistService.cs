using Store.Models.Responses;

namespace Store.Helpers.ProductExistenceService
{
    public interface IProdExistService
    {
        Task<ICollection<ExistenciaResponse>> GetProductExistencesAsync();
    }
}

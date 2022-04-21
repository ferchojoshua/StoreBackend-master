using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ProdMovements
{
    public interface IProductMovementsHelper
    {
        Task<ProductMovments> AddMoverProductAsync(
            AddProductMovementViewModel model,
            Entities.User user
        );

        Task<Existence> UpdateProductExistencesAsync(
            UpdateExistencesByStoreViewModel model,
            Entities.User user
        );
    }
}

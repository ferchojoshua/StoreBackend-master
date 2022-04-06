using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ProductHelper
{
    public interface IProductHelper
    {
        Task<Producto> AddProductAsync(ProductViewModel model);

         Task<Producto> UpdateProductAsync(UpdateProductViewModel model);
    }
}

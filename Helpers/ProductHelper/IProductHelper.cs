using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ProductHelper
{
    public interface IProductHelper
    {
        Task<Producto> AddProductAsync(ProductViewModel model);
        Task<Producto> UpdateProductAsync(UpdateProductViewModel model);
        Task<ICollection<Kardex>> GetKardex(GetKardexViewModel model);
        Task<ICollection<Kardex>> ReparaKardexAsync();
        Task<ICollection<Producto>> GetProdsDifKardex();
        Task<ICollection<Kardex>> GetAllStoresKardex(GetKardexViewModel model);
    }
}

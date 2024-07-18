using Store.Entities;
using Store.Entities.ProductoRecal;
using Store.Models.ViewModels;

namespace Store.Helpers.ProductHelper
{
    public interface IProductHelper
    {
            Task<Producto> AddProductAsync(ProductViewModel model);
            Task<Producto> UpdateProductAsync(UpdateProductViewModel model);
             Task<ICollection<Kardex>> GetKardex(GetKardexViewModel model);
            Task<ICollection<Kardex>> ReparaKardexAsync();
            // Task<ICollection<Producto>> SyncKardexExistencesAsync();
            Task<ICollection<Producto>> GetProdsDifKardex();
            Task<ICollection<Kardex>> GetAllStoresKardex(GetKardexViewModel model);
            Task<ICollection<Producto>> GetProductsRecalByIdAsync(int idStore);
            Task<ProductsRecal> UpdateProductRecallAsync(int Id, int StoreId, int Porcentaje);





    }
}

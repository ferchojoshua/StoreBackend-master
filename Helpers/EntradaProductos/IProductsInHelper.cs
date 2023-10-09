using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.EntradaProductos
{
    public interface IProductsInHelper
    {
        Task<ProductIn> AddProductInAsync(AddEntradaProductoViewModel model, Entities.User user);

        Task<ProductIn> UpdateProductInAsync(
            UpdateEntradaProductoViewModel model,
            Entities.User user
        );
        Task<ProductIn> PagarFacturaAsync(int id);
        
        //Tarea de ObtenerListRecal
        //Task<List<ProductsRecal>> GetAllProductsRecal(int Fam, int TipoNego, int Alm, int ProductoId);
        //Task<List<ProductsRecal>> GetAllProductsRecal();
    }
}

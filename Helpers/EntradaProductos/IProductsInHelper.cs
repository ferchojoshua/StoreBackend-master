using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.EntradaProductos
{
    public interface IProductsInHelper
    {
        Task<ProductIn> AddProductInAsync(AddEntradaProductoViewModel model, string createdBy);
    }
}

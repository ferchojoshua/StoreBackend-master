using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.ProductHelper;
using Store.Models.ViewModels;

namespace Store.Helpers.EntradaProductos
{
    public class ProductHelper : IProductHelper
    {
        private readonly DataContext _context;

        public ProductHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Producto> AddProductAsync(ProductViewModel model)
        {
            Producto producto =
                new()
                {
                    Description = model.Description,
                    Familia = await _context.Familias.FindAsync(model.FamiliaId),
                    TipoNegocio = await _context.TipoNegocios.FindAsync(model.TipoNegocioId),
                    BarCode = model.BarCode,
                    Modelo = model.Modelo,
                    Marca = model.Marca,
                    UM = model.UM,
                    Existencia = 0,
                    PrecioVentaDetalle = 0,
                    PrecioVentaMayor = 0,
                };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateProductAsync(UpdateProductViewModel model)
        {
            Producto producto = await _context.Productos.FindAsync(model.Id);
            producto.Description = model.Description;
            producto.Familia = await _context.Familias.FindAsync(model.FamiliaId);
            producto.TipoNegocio = await _context.TipoNegocios.FindAsync(model.TipoNegocioId);
            producto.BarCode = model.BarCode;
            producto.Modelo = model.Modelo;
            producto.Marca = model.Marca;
            producto.UM = model.UM;
            _context.Entry(producto).State = EntityState.Modified;
             await _context.SaveChangesAsync();
             return producto;
        }
    }
}

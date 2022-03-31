using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.EntradaProductos
{
    public class ProductsInHelper : IProductsInHelper
    {
        private readonly DataContext _context;

        public ProductsInHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductIn> AddProductInAsync(AddEntradaProductoViewModel model, string createdBy)
        {
            DateTime fechaV = DateTime.Now;
            fechaV.AddDays(15);
            Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 1);
            Provider prov = await _context.Providers.FirstOrDefaultAsync(
                p => p.Id == model.ProviderId
            );
            ProductIn productIn = new();
            productIn.TipoEntrada = model.TipoEntrada;
            productIn.TipoPago = model.TipoPago;
            productIn.NoFactura = model.NoFactura;
            productIn.FechaIngreso = DateTime.Now;
            productIn.FechaVencimiento = model.TipoPago == "Pago de Credito" ? fechaV : null;
            productIn.Provider = prov;
            productIn.Almacen = alm;
            productIn.CreatedBy = createdBy; 
            productIn.MontoFactura = model.MontoFactura;
            List<ProductInDetails> detalles = new();
            foreach (var item in model.ProductInDetails)
            {
                ProductInDetails pd = new();
                pd.Product = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );
                pd.Cantidad = item.Cantidad;
                pd.CostoCompra = item.CostoCompra;
                pd.CostoUnitario = item.CostoUnitario;
                pd.Descuento = item.Descuento;
                pd.Impuesto = item.Impuesto;
                pd.PrecioVentaMayor = item.PrecioVentaMayor;
                pd.PrecioVentaDetalle = item.PrecioVentaDetalle;
                detalles.Add(pd);
            }
            productIn.ProductInDetails = detalles;
            _context.ProductIns.Add(productIn);
            await _context.SaveChangesAsync();

            return productIn;
        }
    }
}

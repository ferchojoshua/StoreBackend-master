using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ProductHelper
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
                };

            _context.Productos.Add(producto);

            Existence existence =
                new()
                {
                    Almacen = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 4),
                    Producto = producto,
                    Existencia = 0,
                    PrecioVentaMayor = 0,
                    PrecioVentaDetalle = 0
                };

            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<ICollection<Kardex>> GetAllStoresKardex(GetKardexViewModel model)
        {
            return await _context.Kardex
                .Include(k => k.User)
                .Where(
                    k =>
                        k.Product.Id == model.ProductId
                        && k.Fecha.DayOfYear >= model.Desde.DayOfYear
                        && k.Fecha.DayOfYear <= model.Hasta.DayOfYear
                )
                .ToListAsync();
        }

        public async Task<ICollection<Kardex>> GetKardex(GetKardexViewModel model)
        {
            return await _context.Kardex
                .Include(k => k.User)
                .Where(
                    k =>
                        k.Product.Id == model.ProductId
                        && k.Almacen.Id == model.StoreId
                        && k.Fecha.DayOfYear >= model.Desde.DayOfYear
                        && k.Fecha.DayOfYear <= model.Hasta.DayOfYear
                )
                .ToListAsync();
        }

        public async Task<ICollection<Kardex>> ReparaKardexAsync()
        {
            List<Kardex> kardexByStore = new();
            var kardexList = await _context.Kardex.Include(k => k.Product).ToListAsync();
            var prod = kardexList.GroupBy(x => x.Product).Select(x => new { ProductId = x.Key.Id });
            var storeList = await _context.Almacen.ToListAsync();

            foreach (var item in prod)
            {
                foreach (var store in storeList)
                {
                    int entrada = 0;
                    int salida = 0;
                    int saldo = 0;

                    var kardexMov = await _context.Kardex
                        .Where(k => k.Almacen.Id == store.Id && k.Product.Id == item.ProductId)
                        .ToListAsync();

                    if (kardexMov != null)
                    {
                        foreach (var mov in kardexMov)
                        {
                            int saldoMov = 0;
                            entrada = mov.Entradas;
                            salida = mov.Salidas;
                            saldoMov = mov.Saldo;
                            saldo += entrada - salida;
                            if (saldo != saldoMov)
                            {
                                mov.Saldo = saldo;
                                _context.Entry(mov).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            return kardexByStore;
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

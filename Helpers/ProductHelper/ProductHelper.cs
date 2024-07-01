using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Entities.ProductoRecal;
using Store.Models.ViewModels;
using Store.Models.ViewModels.Masivo;
using System.Data;

namespace Store.Helpers.ProductHelper
{
    public class ProductHelper :   IProductHelper
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
                .Include(k => k.AjusteInventario)
                .Include(k => k.EntradaProduct)
                .Include(k => k.Sale)
                .Include(k => k.SaleAnulation)
                .Include(k => k.TrasladoInventario)
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
                        //&& k.Fecha.DayOfYear >= model.Desde.DayOfYear
                        //&& k.Fecha.DayOfYear <= model.Hasta.DayOfYear

                )
                .ToListAsync();
        }

        public async Task<ICollection<Producto>> GetProdsDifKardex()
        {
            List<Producto> result = new();

            var kardexList = await _context.Kardex.Include(k => k.Product).ToListAsync();
            var prod = kardexList.GroupBy(x => x.Product).Select(x => new { ProductId = x.Key.Id });
            var storeList = await _context.Almacen.ToListAsync();
            foreach (var item in prod)
            {
                foreach (var store in storeList)
                {
                    try
                    {
                        Kardex kar = await _context.Kardex
                            .Where(k => k.Product.Id == item.ProductId && k.Almacen == store)
                            .OrderByDescending(k => k.Id)
                            .FirstOrDefaultAsync();

                        Existence exist = await _context.Existences
                            .Where(e => e.Producto.Id == item.ProductId && e.Almacen == store)
                            .FirstOrDefaultAsync();
                        if (kar != null && exist != null)
                        {
                            if (exist.Existencia != kar.Saldo)
                            {
                                result.Add(exist.Producto);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            return result;
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


        // public Task<ICollection<Producto>> SyncKardexExistencesAsync()
        // {
        //     List<Producto> updatedProducts = new();
        //     // var prodList = await _context.Productos.ToListAsync();
        //     // var storeList = await _context.Almacen.ToListAsync();
        //     // foreach (var prod in prodList)
        //     // {
        //     //     foreach (var store in storeList)
        //     //     {
        //     //         var entrada = await _context.ProductIns.Where(pi =>  == prod.Id && pi.)
        //     //             .OrderBy(pi => pi.Id)
        //     //             .ToListAsync();

        //     //         var detalleVenta = await _context.SaleDetails
        //     //             .OrderBy(sd => sd.Id)
        //     //             .ToListAsync();

        //     //         var movmentDetails = await _context.ProductMovmentDetails
        //     //             .OrderBy(pm => pm.Id)
        //     //             .ToListAsync();
        //     //     }
        //     // }

        //     // foreach (var item in prod)
        //     // {
        //     //     foreach (var store in storeList)
        //     //     {
        //     //         var movmentsList = await _context.ProductMovmentDetails
        //     //             .Include(m => m.ProductMovment)
        //     //             .Where(
        //     //                 pm =>
        //     //                     pm.Producto.Id == item.ProductId && pm.AlmacenDestinoId == store.Id
        //     //             )
        //     //             .OrderBy(m => m.Id)
        //     //             .ToListAsync();

        //     //         var kardexMov = await _context.Kardex
        //     //             .Where(
        //     //                 k =>
        //     //                     k.Almacen.Id == store.Id
        //     //                     && k.Product.Id == item.ProductId
        //     //                     && k.Concepto.Contains("TRASLADO DE INVENTARIO")
        //     //             )
        //     //             .OrderBy(k => k.Id)
        //     //             .ToListAsync();

        //     //         bool isUpdated = false;

        //     //         for (int i = 0; i < movmentsList.Count; i++)
        //     //         {
        //     //             if (movmentsList.Count != 0)
        //     //             {
        //     //                 var kardex = await _context.Kardex
        //     //                     .Where(
        //     //                         k =>
        //     //                             k.Almacen.Id == store.Id
        //     //                             && k.Product.Id == item.ProductId
        //     //                             && k.Concepto.Contains("TRASLADO DE INVENTARIO")
        //     //                             && k.Fecha.Date == movmentsList[i].ProductMovment.Fecha.Date
        //     //                             && k.Entradas != 0
        //     //                     )
        //     //                     .FirstOrDefaultAsync();
        //     //                 if (movmentsList[i].Cantidad != kardex.Entradas)
        //     //                 {
        //     //                     if (movmentsList.Count == kardexMov.Count)
        //     //                     {
        //     //                         if (kardexMov[i].Salidas == 0)
        //     //                         {
        //     //                             kardexMov[i].Entradas = movmentsList[i].Cantidad;
        //     //                             _context.Entry(kardexMov[i]).State = EntityState.Modified;
        //     //                             await _context.SaveChangesAsync();
        //     //                             updatedProducts.Add(kardexMov[i].Product);
        //     //                             isUpdated = true;
        //     //                         }
        //     //                         if (isUpdated)
        //     //                         {
        //     //                             int entrada = 0;
        //     //                             int salida = 0;
        //     //                             int saldo = 0;

        //     //                             var reparar = await _context.Kardex
        //     //                                 .Where(
        //     //                                     k =>
        //     //                                         k.Almacen.Id == store.Id
        //     //                                         && k.Product.Id == item.ProductId
        //     //                                 )
        //     //                                 .ToListAsync();
        //     //                             foreach (var mov in reparar)
        //     //                             {
        //     //                                 int saldoMov = 0;
        //     //                                 entrada = mov.Entradas;
        //     //                                 salida = mov.Salidas;
        //     //                                 saldoMov = mov.Saldo;
        //     //                                 saldo += entrada - salida;
        //     //                                 if (saldo != saldoMov)
        //     //                                 {
        //     //                                     mov.Saldo = saldo;
        //     //                                     _context.Entry(mov).State = EntityState.Modified;
        //     //                                     await _context.SaveChangesAsync();
        //     //                                 }
        //     //                             }
        //     //                         }
        //     //                     }
        //     //                     else
        //     //                     {
        //     //                         if (kardex.Salidas == 0)
        //     //                         {
        //     //                             kardex.Entradas = movmentsList[i].Cantidad;
        //     //                             _context.Entry(kardex).State = EntityState.Modified;
        //     //                             await _context.SaveChangesAsync();
        //     //                             updatedProducts.Add(kardex.Product);
        //     //                             isUpdated = true;
        //     //                         }
        //     //                         if (isUpdated)
        //     //                         {
        //     //                             int entrada = 0;
        //     //                             int salida = 0;
        //     //                             int saldo = 0;

        //     //                             var reparar = await _context.Kardex
        //     //                                 .Where(
        //     //                                     k =>
        //     //                                         k.Almacen.Id == store.Id
        //     //                                         && k.Product.Id == item.ProductId
        //     //                                 )
        //     //                                 .ToListAsync();
        //     //                             foreach (var mov in reparar)
        //     //                             {
        //     //                                 int saldoMov = 0;
        //     //                                 entrada = mov.Entradas;
        //     //                                 salida = mov.Salidas;
        //     //                                 saldoMov = mov.Saldo;
        //     //                                 saldo += entrada - salida;
        //     //                                 if (saldo != saldoMov)
        //     //                                 {
        //     //                                     mov.Saldo = saldo;
        //     //                                     _context.Entry(mov).State = EntityState.Modified;
        //     //                                     await _context.SaveChangesAsync();
        //     //                                 }
        //     //                             }
        //     //                         }
        //     //                     }
        //     //                 }
        //     //             }
        //     //         }

        //     //         // foreach (var movmen in movmentsList)
        //     //         // {
        //     //         //     var kardexMov = await _context.Kardex
        //     //         //         .Where(k => k.Almacen.Id == store.Id && k.Product.Id == item.ProductId)
        //     //         //         .ToListAsync();
        //     //         // if (kardexMov != null)
        //     //         // {
        //     //         //     foreach (var mov in kardexMov)
        //     //         //     {
        //     //         //         if (mov.Concepto.Contains("TRASLADO DE INVENTARIO"))
        //     //         //         {
        //     //         //             int entrada = 0;
        //     //         //             int salida = 0;
        //     //         //             int saldo = 0;

        //     //         //             int iO = Math.Abs(mov.Entradas - mov.Salidas);
        //     //         //             if (movmen.Cantidad != iO)
        //     //         //             {
        //     //         //                 int saldoMov = 0;
        //     //         //                 entrada = movmen.Cantidad;
        //     //         //                 salida = mov.Salidas;
        //     //         //                 saldoMov = mov.Saldo;
        //     //         //                 saldo += entrada - salida;
        //     //         //                 break;
        //     //         //             }
        //     //         //             else
        //     //         //             {
        //     //         //                 continue;
        //     //         //             }
        //     //         //         }
        //     //         //     }
        //     //         // }
        //     //         // }
        //     //         // int entrada = 0;
        //     //         // int salida = 0;
        //     //         // int saldo = 0;

        //     //         // if (kardexMov != null)
        //     //         // {
        //     //         //     foreach (var mov in kardexMov)
        //     //         //     {
        //     //         //         int saldoMov = 0;
        //     //         //         entrada = mov.Entradas;
        //     //         //         salida = mov.Salidas;
        //     //         //         saldoMov = mov.Saldo;
        //     //         //         saldo += entrada - salida;
        //     //         //         if (saldo != saldoMov)
        //     //         //         {
        //     //         //             mov.Saldo = saldo;
        //     //         //             _context.Entry(mov).State = EntityState.Modified;
        //     //         //             await _context.SaveChangesAsync();
        //     //         //         }
        //     //         //     }
        //     //         // }
        //     //     }
        //     // }

        //     // return updatedProducts;
        // }

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

        //public async Task<ICollection<Producto>> GetProductsRecalByIdAsync(ProductViewModel model)
        //    public async Task<Producto> GetProductsRecalByIdAsync(ProductViewModel model)
        //{
               
        //}

      public async Task<ICollection<Producto>> GetProductsRecalByIdAsync(int idStore)

        {
            var storeList = await _context.Almacen.ToListAsync();
            return await _context.Productos.Where(r=>r.Existences.FirstOrDefault().Almacen.Id == idStore)
             .Include(p => p.TipoNegocio)
             .Include(p => p.Familia)
             .Include(p => p.Existences)
             .Select(
                 x =>
                     new Producto()
                     {
                         Id = x.Id,
                         TipoNegocio = new TipoNegocio()
                         {
                             Id = x.TipoNegocio.Id,
                             Description = x.TipoNegocio.Description
                         },
                         Familia = x.Familia,
                         Description = x.Description,
                         BarCode = x.BarCode,
                         Marca = x.Marca,
                         Modelo = x.Modelo,
                         UM = x.UM
                     }
             )
             .OrderBy(p => p.Description)
             .ToListAsync();
        }
        public async Task<ProductsRecal> UpdateProductRecallAsync(int Id, int StoreId, int Porcentaje)
        {
            ProductsRecal ProductRecal = new ProductsRecal
            {
                Id = Id,
                StoreId = StoreId,
                Porcentaje = Porcentaje,
            };

            try
            {
                _context.ProductsRecal.Add(ProductRecal);
                await _context.SaveChangesAsync();

                var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            EXEC [dbo].[uspProductsupdate] {ProductRecal.Id}, {StoreId}, {Porcentaje}, @Mensaje OUTPUT;
        ");

                string mensaje = mensajeParam.Value.ToString();

                if (string.IsNullOrEmpty(mensaje) || !mensaje.Contains("exitosamente"))
                {
                    throw new Exception($"Error al actualizar: {mensaje}");
                }
                else
                {
                    return ProductRecal;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
            }
        }

                

    }
}
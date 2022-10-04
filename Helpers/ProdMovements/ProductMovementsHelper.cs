using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ProdMovements
{
    public class ProductMovementsHelper : IProductMovementsHelper
    {
        private readonly DataContext _context;

        public ProductMovementsHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductMovments> AddMoverProductAsync(
            AddProductMovementViewModel model,
            Entities.User user
        )
        {
            List<ProductMovmentDetails> pMList = new();
            List<Kardex> KardexMovments = new();

            foreach (var item in model.MovmentDetails)
            {
                Producto prod = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.IdProducto
                );
                ProductMovmentDetails pMDetails =
                    new()
                    {
                        Producto = prod,
                        AlmacenProcedenciaId = item.AlmacenProcedenciaId,
                        AlmacenDestinoId = item.AlmacenDestinoId,
                        Cantidad = item.Cantidad,
                    };

                pMList.Add(pMDetails);

                //Aca Buscamos la existencia en el almacen prodecencia
                Existence existProcedencia = await _context.Existences
                    .Where(
                        e =>
                            e.Producto.Id == item.IdProducto
                            && e.Almacen.Id == item.AlmacenProcedenciaId
                    )
                    .FirstOrDefaultAsync();

                //if is null or procedence is major than 0 return null
                if (existProcedencia == null || existProcedencia.Existencia <= 0)
                {
                    return null;
                }
                else
                {
                    existProcedencia.Existencia -= item.Cantidad;
                    if (existProcedencia.Existencia < 0)
                    {
                        return null;
                    }

                    //Aca Buscamos la existencia en el almacen prodecencia
                    Existence existDestino = await _context.Existences.FirstOrDefaultAsync(
                        e =>
                            e.Producto.Id == item.IdProducto
                            && e.Almacen.Id == item.AlmacenDestinoId
                    );

                    //si es nulo se crea nuevo y se agrega a la DB
                    if (existDestino == null)
                    {
                        existDestino = new()
                        {
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(
                                a => a.Id == item.AlmacenDestinoId
                            ),
                            Producto = prod,
                            Existencia = item.Cantidad,
                            PrecioCompra = existProcedencia.PrecioCompra,
                            PrecioVentaDetalle = existProcedencia.PrecioVentaDetalle,
                            PrecioVentaMayor = existProcedencia.PrecioVentaMayor,
                            Maximo = 0,
                            Minimo = 0
                        };
                        _context.Existences.Add(existDestino);
                    }
                    //si no es nulo, se edita el existente
                    else
                    {
                        existDestino.Existencia += item.Cantidad;
                        existDestino.PrecioVentaDetalle = existProcedencia.PrecioVentaDetalle;
                        existDestino.PrecioVentaMayor = existProcedencia.PrecioVentaMayor;
                        existDestino.PrecioCompra = existProcedencia.PrecioCompra;
                        _context.Entry(existDestino).State = EntityState.Modified;
                    }

                    Kardex kar = await _context.Kardex
                        .Where(k => k.Product == prod && k.Almacen == existProcedencia.Almacen)
                        .OrderByDescending(k => k.Id)
                        .FirstOrDefaultAsync();

                    int saldo = kar == null ? 0 : kar.Saldo;

                    Kardex kardexProcedencia =
                        new()
                        {
                            Product = prod,
                            Fecha = DateTime.Now,
                            Concepto = "TRASLADO DE INVENTARIO",
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(
                                a => a.Id == item.AlmacenProcedenciaId
                            ),
                            Entradas = 0,
                            Salidas = item.Cantidad,
                            Saldo = saldo - item.Cantidad,
                            User = user
                        };
                    // _context.Kardex.Add(kardexProcedencia);
                    KardexMovments.Add(kardexProcedencia);

                    //Agregamos el Kardex de entrada al almacen destino
                    Kardex karDest = await _context.Kardex
                        .Where(k => k.Product == prod && k.Almacen == existDestino.Almacen)
                        .OrderByDescending(k => k.Id)
                        .FirstOrDefaultAsync();

                    int saldoDest = karDest == null ? 0 : karDest.Saldo;

                    Kardex kardex =
                        new()
                        {
                            Product = prod,
                            Fecha = DateTime.Now,
                            Concepto = "TRASLADO DE INVENTARIO",
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(
                                a => a.Id == item.AlmacenDestinoId
                            ),
                            Entradas = item.Cantidad,
                            Salidas = 0,
                            Saldo = saldoDest + item.Cantidad,
                            User = user
                        };
                    // _context.Kardex.Add(kardex);
                    KardexMovments.Add(kardex);
                }

                _context.Entry(existProcedencia).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            //Aca registramos el movimiento del producto
            ProductMovments productMovments =
                new()
                {
                    User = user,
                    Concepto = model.Concepto,
                    Fecha = DateTime.Now,
                    MovmentDetails = pMList,
                    KardexMovments = KardexMovments
                };

            _context.ProductMovments.Add(productMovments);
            await _context.SaveChangesAsync();
            return productMovments;
        }

        public async Task<ICollection<ProductMovments>> GetProductMovmentsAsync()
        {
            return await _context.ProductMovments
                .Include(p => p.MovmentDetails)
                .ThenInclude(md => md.Producto)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Existence> UpdateProductExistencesAsync(
            UpdateExistencesByStoreViewModel model,
            Entities.User user
        )
        {
            DateTime hoy = DateTime.Now;
            Existence exist = await _context.Existences
                .Include(e => e.Producto)
                .Include(e => e.Almacen)
                .FirstOrDefaultAsync(p => p.Id == model.Id);
            if (exist == null)
            {
                return exist;
            }
            List<StockAdjustmentDetail> stockAdjustmentDetails = new();

            List<Kardex> KardexMovments = new();

            int diferencia = model.NewExistencias - exist.Existencia;
            decimal sumarotiaMontoCompra = 0;
            decimal sumatoriaMontoVenta = 0;

            //Este solo modifica la existencia, el kardex no
            if (diferencia == 0)
            {
                exist.PrecioVentaDetalle = model.NewPVD;
                exist.PrecioVentaMayor = model.NewPVM;
            }
            else if (diferencia < 0)
            {
                exist.PrecioVentaDetalle = model.NewPVD;
                exist.PrecioVentaMayor = model.NewPVM;
                exist.Existencia = model.NewExistencias;

                sumarotiaMontoCompra += exist.PrecioCompra * Math.Abs(diferencia);
                sumatoriaMontoVenta += exist.PrecioVentaMayor * Math.Abs(diferencia);

                StockAdjustmentDetail detalle =
                    new()
                    {
                        Store = exist.Almacen,
                        Product = exist.Producto,
                        Cantidad = Math.Abs(diferencia),
                        PrecioCompra = exist.PrecioCompra,
                        MontoFinalCompra = exist.PrecioCompra * Math.Abs(diferencia),
                        PrecioUnitarioVenta = exist.PrecioVentaMayor,
                        MontoFinalVenta = exist.PrecioVentaMayor * Math.Abs(diferencia),
                    };
                stockAdjustmentDetails.Add(detalle);

                //Agregamos el Kardex de entrada al almacen destino
                Kardex kar = await _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .OrderByDescending(k => k.Id)
                    .FirstOrDefaultAsync();

                int saldo = kar == null ? 0 : kar.Saldo;

                Kardex kardex =
                    new()
                    {
                        Product = exist.Producto,
                        Fecha = hoy,
                        Concepto = "AJUSTE DE INVENTARIO",
                        Almacen = exist.Almacen,
                        Entradas = 0,
                        Salidas = Math.Abs(diferencia),
                        Saldo = saldo + diferencia,
                        User = user
                    };
                // _context.Kardex.Add(kardex);
                KardexMovments.Add(kardex);
            }
            else
            {
                exist.PrecioVentaDetalle = model.NewPVD;
                exist.PrecioVentaMayor = model.NewPVM;
                exist.Existencia = model.NewExistencias;

                sumarotiaMontoCompra += exist.PrecioCompra * Math.Abs(diferencia);
                sumatoriaMontoVenta += exist.PrecioVentaMayor * Math.Abs(diferencia);

                StockAdjustmentDetail detalle =
                    new()
                    {
                        Store = exist.Almacen,
                        Product = exist.Producto,
                        Cantidad = Math.Abs(diferencia),
                        PrecioCompra = exist.PrecioCompra,
                        MontoFinalCompra = exist.PrecioCompra * Math.Abs(diferencia),
                        PrecioUnitarioVenta = exist.PrecioVentaMayor,
                        MontoFinalVenta = exist.PrecioVentaMayor * Math.Abs(diferencia),
                    };
                stockAdjustmentDetails.Add(detalle);

                //Agregamos el Kardex de entrada al almacen destino
                Kardex kar = await _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .OrderByDescending(k => k.Id)
                    .FirstOrDefaultAsync();

                int saldo = kar == null ? 0 : kar.Saldo;

                Kardex kardex =
                    new()
                    {
                        Product = exist.Producto,
                        Fecha = hoy,
                        Concepto = "AJUSTE DE INVENTARIO",
                        Almacen = exist.Almacen,
                        Entradas = diferencia,
                        Salidas = 0,
                        Saldo = saldo + diferencia,
                        User = user
                    };
                // _context.Kardex.Add(kardex);
                KardexMovments.Add(kardex);
            }

            StockAdjustment stockAdjustment =
                new()
                {
                    Fecha = hoy,
                    RealizadoPor = user,
                    StockAdjustmentDetails = stockAdjustmentDetails,
                    KardexMovments = KardexMovments,
                    MontoPrecioCompra = sumarotiaMontoCompra,
                    MontoPrecioVenta = sumatoriaMontoVenta,
                    Store = exist.Almacen
                };

            _context.Entry(exist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return exist;
        }
    }
}

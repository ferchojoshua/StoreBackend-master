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
            foreach (var item in model.MovmentDetails)
            {
                string almacenText = "";
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
                    .Where(e => e.Producto == prod && e.Almacen.Id == item.AlmacenProcedenciaId)
                    .FirstOrDefaultAsync();

                //if is null or procedence is major than 0 return null
                if (existProcedencia == null || existProcedencia.Existencia <= 0)
                {
                    return null;
                }
                else
                {
                    if (existProcedencia.Existencia < item.Cantidad)
                    {
                        return null;
                    }

                    //Aca Buscamos la existencia en el almacen prodecencia
                    Existence existDestino = await _context.Existences.FirstOrDefaultAsync(
                        e => e.Producto == prod && e.Almacen.Id == item.AlmacenDestinoId
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
                            PrecioVentaDetalle = existProcedencia.PrecioVentaDetalle,
                            PrecioVentaMayor = existProcedencia.PrecioVentaMayor,
                            PrecioCompra = existProcedencia.PrecioCompra
                        };
                        almacenText = existDestino.Almacen.Name;
                        _context.Existences.Add(existDestino);
                    }
                    //si no es nulo, se edita el existente
                    else
                    {
                        existDestino.Existencia += item.Cantidad;
                        existDestino.PrecioVentaDetalle = existProcedencia.PrecioVentaDetalle;
                        existDestino.PrecioVentaMayor = existProcedencia.PrecioVentaMayor;
                        almacenText = existDestino.Almacen.Name;
                        existDestino.PrecioCompra = existProcedencia.PrecioCompra;
                        _context.Entry(existDestino).State = EntityState.Modified;
                    }

                    Kardex kar = await _context.Kardex
                        .Where(k => k.Product == prod && k.Almacen == existProcedencia.Almacen)
                        .OrderByDescending(k => k.Id)
                        .FirstOrDefaultAsync();

                    Kardex kardexProcedencia =
                        new()
                        {
                            Product = prod,
                            Fecha = DateTime.Now,
                            Concepto = $"TRASLADO DE INVENTARIO A - {almacenText}",
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(
                                a => a.Id == item.AlmacenProcedenciaId
                            ),
                            Entradas = 0,
                            Salidas = item.Cantidad,
                            Saldo = kar == null ? 0 : kar.Saldo - item.Cantidad,
                            User = user
                        };
                    _context.Kardex.Add(kardexProcedencia);

                    //Agregamos el Kardex de entrada al almacen destino
                    Kardex karDest = await _context.Kardex
                        .Where(k => k.Product == prod && k.Almacen == existDestino.Almacen)
                        .OrderByDescending(k => k.Id)
                        .FirstOrDefaultAsync();

                    Kardex kardex =
                        new()
                        {
                            Product = prod,
                            Fecha = DateTime.Now,
                            Concepto = $"TRASLADO DE INVENTARIO A - {almacenText}",
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(
                                a => a.Id == item.AlmacenDestinoId
                            ),
                            Entradas = item.Cantidad,
                            Salidas = 0,
                            Saldo = karDest == null ? 0 : karDest.Saldo + item.Cantidad,
                            User = user
                        };
                    _context.Kardex.Add(kardex);
                }

                existProcedencia.Existencia -= item.Cantidad;
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
                    MovmentDetails = pMList
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
            Existence exist = await _context.Existences
                .Include(e => e.Producto)
                .Include(e => e.Almacen)
                .FirstOrDefaultAsync(p => p.Id == model.Id);
            if (exist == null)
            {
                return exist;
            }
            int diferencia = model.NewExistencias - exist.Existencia;

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

                //Agregamos el Kardex de entrada al almacen destino
                var karList = await _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .ToListAsync();

                Kardex kar = karList.Where(k => k.Id == karList.Max(k => k.Id)).FirstOrDefault();

                Kardex kardex =
                    new()
                    {
                        Product = exist.Producto,
                        Fecha = DateTime.Now,
                        Concepto = "AJUSTE DE INVENTARIO",
                        Almacen = exist.Almacen,
                        Entradas = 0,
                        Salidas = Math.Abs(diferencia),
                        Saldo = kar.Saldo + diferencia,
                        User = user
                    };
                _context.Kardex.Add(kardex);
            }
            else
            {
                exist.PrecioVentaDetalle = model.NewPVD;
                exist.PrecioVentaMayor = model.NewPVM;
                exist.Existencia = model.NewExistencias;

                //Agregamos el Kardex de entrada al almacen destino
                var karList = await _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .ToListAsync();

                Kardex kar = karList.Where(k => k.Id == karList.Max(k => k.Id)).FirstOrDefault();

                Kardex kardex =
                    new()
                    {
                        Product = exist.Producto,
                        Fecha = DateTime.Now,
                        Concepto = "AJUSTE DE INVENTARIO",
                        Almacen = exist.Almacen,
                        Entradas = diferencia,
                        Salidas = 0,
                        Saldo = kar.Saldo + diferencia,
                        User = user
                    };
                _context.Kardex.Add(kardex);
            }

            _context.Entry(exist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return exist;
        }
    }
}

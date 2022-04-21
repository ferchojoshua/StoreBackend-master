using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.ProductHelper;
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
            string almacenText = "";
            Producto prod = await _context.Productos.FirstOrDefaultAsync(
                p => p.Id == model.IdProducto
            );

            //Aca registramos el movimiento del producto
            ProductMovments pM =
                new()
                {
                    Producto = prod,
                    AlmacenProcedenciaId = model.AlmacenProcedenciaId,
                    AlmacenDestinoId = model.AlmacenDestinoId,
                    Cantidad = model.Cantidad,
                    Concepto = model.Concepto,
                    User = user,
                    Fecha = DateTime.Now
                };

            _context.ProductMovments.Add(pM);

            //Aca Buscamos la existencia en el almacen prodecencia
            Existence existProcedencia = await _context.Existences
                .Include(e => e.Almacen)
                .Include(e => e.Producto)
                .Where(
                    e => e.Producto.Id == pM.Producto.Id && e.Almacen.Id == pM.AlmacenProcedenciaId
                )
                .FirstOrDefaultAsync();

            //if is null or procedence is major than 0 return null
            if (existProcedencia == null || existProcedencia.Existencia <= 0)
            {
                return pM = null;
            }
            else
            {
                if (existProcedencia.Existencia < pM.Cantidad)
                {
                    return pM = null;
                }

                //Aca Buscamos la existencia en el almacen prodecencia
                Existence existDestino = await _context.Existences
                    .Include(e => e.Almacen)
                    .Include(e => e.Producto)
                    .FirstOrDefaultAsync(
                        e => e.Producto == pM.Producto && e.Almacen.Id == pM.AlmacenDestinoId
                    );

                //si es nulo se crea nuevo y se agrega a la DB
                if (existDestino == null)
                {
                    existDestino = new()
                    {
                        Almacen = await _context.Almacen.FirstOrDefaultAsync(
                            a => a.Id == pM.AlmacenDestinoId
                        ),
                        Producto = pM.Producto,
                        Existencia = pM.Cantidad,
                        PrecioVentaDetalle = existProcedencia.PrecioVentaDetalle,
                        PrecioVentaMayor = existProcedencia.PrecioVentaMayor
                    };
                    almacenText = existDestino.Almacen.Name;
                    _context.Existences.Add(existDestino);
                }
                //si no es nulo, se edita el existente
                else
                {
                    existDestino.Existencia += pM.Cantidad;
                    existDestino.PrecioVentaDetalle = existProcedencia.PrecioVentaDetalle;
                    existDestino.PrecioVentaMayor = existProcedencia.PrecioVentaMayor;
                    almacenText = existDestino.Almacen.Name;
                    _context.Entry(existDestino).State = EntityState.Modified;
                }

                //Agregamos el Kardex de entrada al almacen destino
                int totalEntradas = _context.Kardex
                    .Where(k => k.Product.Id == pM.Producto.Id && k.Almacen == existDestino.Almacen)
                    .Sum(k => k.Entradas);

                int totaSalidas = _context.Kardex
                    .Where(k => k.Product.Id == pM.Producto.Id && k.Almacen == existDestino.Almacen)
                    .Sum(k => k.Salidas);

                int saldo = totalEntradas - totaSalidas;

                Kardex kardex =
                    new()
                    {
                        Product = prod,
                        Fecha = DateTime.Now,
                        Concepto = $"TRASLADO DE INVENTARIO A - {almacenText}",
                        Almacen = existDestino.Almacen,
                        Entradas = existDestino.Existencia,
                        Salidas = 0,
                        Saldo = saldo + existDestino.Existencia,
                        User = user
                    };
                _context.Kardex.Add(kardex);
            }

            existProcedencia.Existencia -= pM.Cantidad;
            _context.Entry(existProcedencia).State = EntityState.Modified;

            //Agregamos el Kardex de salida al almacen procedencia
            int totalEntradasProcedencia = _context.Kardex
                .Where(k => k.Product.Id == pM.Producto.Id && k.Almacen == existProcedencia.Almacen)
                .Sum(k => k.Entradas);

            int totaSalidasProcedencia = _context.Kardex
                .Where(k => k.Product.Id == pM.Producto.Id && k.Almacen == existProcedencia.Almacen)
                .Sum(k => k.Salidas);

            int saldoProcedencia = totalEntradasProcedencia - totaSalidasProcedencia;

            Kardex kardexProcedencia =
                new()
                {
                    Product = prod,
                    Fecha = DateTime.Now,
                    Concepto = $"TRASLADO DE INVENTARIO A - {almacenText}",
                    Almacen = existProcedencia.Almacen,
                    Entradas = 0,
                    Salidas = model.Cantidad,
                    Saldo = saldoProcedencia - model.Cantidad,
                    User = user
                };
            _context.Kardex.Add(kardexProcedencia);

            await _context.SaveChangesAsync();
            return pM;
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
                int totalEntradas = _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .Sum(k => k.Entradas);

                int totaSalidas = _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .Sum(k => k.Salidas);

                int saldo = totalEntradas - totaSalidas;

                Kardex kardex =
                    new()
                    {
                        Product = exist.Producto,
                        Fecha = DateTime.Now,
                        Concepto = "AJUSTE DE INVENTARIO",
                        Almacen = exist.Almacen,
                        Entradas = 0,
                        Salidas = diferencia,
                        Saldo = saldo + diferencia,
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
                int totalEntradas = _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .Sum(k => k.Entradas);

                int totaSalidas = _context.Kardex
                    .Where(k => k.Product.Id == exist.Producto.Id && k.Almacen == exist.Almacen)
                    .Sum(k => k.Salidas);

                int saldo = totalEntradas - totaSalidas;

                Kardex kardex =
                    new()
                    {
                        Product = exist.Producto,
                        Fecha = DateTime.Now,
                        Concepto = "AJUSTE DE INVENTARIO",
                        Almacen = exist.Almacen,
                        Entradas = diferencia,
                        Salidas = 0,
                        Saldo = saldo + diferencia,
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

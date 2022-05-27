using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public class SalesService : ISalesService
    {
        private readonly DataContext _context;

        public SalesService(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Sales>> GetContadoSalesByStoreAsync(int idStore)
        {
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado == false && s.Store.Id == idStore && s.IsContado)
                .ToListAsync();
        }

        public async Task<ICollection<Sales>> GetCreditoSalesByStoreAsync(int idStore)
        {
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado == false && s.Store.Id == idStore && s.IsContado == false)
                .ToListAsync();
        }

        public async Task<ICollection<Sales>> GetAnulatedSalesByStoreAsync(int idStore)
        {
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado))
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado && s.Store.Id == idStore)
                .ToListAsync();
        }

        public async Task<ICollection<Sales>> GetAnuledSalesListAsync()
        {
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado == true)
                .ToListAsync();
        }

        public async Task<Sales> AddSaleAsync(AddSaleViewModel model, Entities.User user)
        {
            Client cl = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient);
            if (cl != null)
            {
                cl.ContadorCompras += 1;
                _context.Entry(cl).State = EntityState.Modified;
            }

            Sales sale =
                new()
                {
                    IsEventual = model.IsEventual,
                    NombreCliente = model.IsEventual ? model.NombreCliente : "",
                    Client = cl,
                    ProductsCount = model.SaleDetails.Count,
                    MontoVenta = model.MontoVenta,
                    FechaVenta = DateTime.Now,
                    FacturedBy = user,
                    IsContado = model.IsContado,
                    IsCanceled = model.IsContado, //Si es de contado, esta cancelado
                    Saldo = model.IsContado ? 0 : model.MontoVenta,
                    FechaVencimiento = DateTime.Now.AddDays(15),
                    Store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.Storeid)
                };

            List<SaleDetail> detalles = new();
            foreach (var item in model.SaleDetails)
            {
                Producto prod = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );
                Almacen alm = await _context.Almacen.FirstOrDefaultAsync(
                    a => a.Id == item.Store.Id
                );
                //Se crea el objeto detalle venta
                SaleDetail saleDetail =
                    new()
                    {
                        Store = alm,
                        Product = prod,
                        Cantidad = item.Cantidad,
                        Descuento = item.Descuento,
                        CostoUnitario = item.CostoUnitario,
                        PVM = item.PVM,
                        PVD = item.PVD,
                        CostoTotal = item.CostoTotal
                    };
                detalles.Add(saleDetail); //Se agrega a la lista

                //Modificamos las existencias
                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == prod && e.Almacen == alm
                );
                existence.Existencia -= saleDetail.Cantidad;
                existence.PrecioVentaDetalle = item.PVD;
                existence.PrecioVentaMayor = item.PVM;
                _context.Entry(existence).State = EntityState.Modified;

                //Agregamos el Kardex de entrada al almacen destino
                var karList = await _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                    .ToListAsync();

                Kardex kar = karList.Where(k => k.Id == karList.Max(k => k.Id)).FirstOrDefault();

                Kardex kardex =
                    new()
                    {
                        Product = prod,
                        Fecha = DateTime.Now,
                        Concepto = model.IsContado ? "VENTA DE CONTADO" : "VENTA DE CREDITO",
                        Almacen = alm,
                        Entradas = 0,
                        Salidas = item.Cantidad,
                        Saldo = kar.Saldo - item.Cantidad,
                        User = user
                    };
                _context.Kardex.Add(kardex);
            }

            if (sale.IsContado)
            {
                Abono abono =
                    new()
                    {
                        Sale = sale,
                        Monto = sale.MontoVenta,
                        RealizedBy = user,
                        FechaAbono = DateTime.Now,
                        Store = detalles[0].Store
                    };
                _context.Abonos.Add(abono);
            }

            //Unificamos objetos y los mandamos a la DB
            sale.SaleDetails = detalles;
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<ICollection<Abono>> GetQuoteListAsync(int id)
        {
            return await _context.Abonos
                .Include(a => a.Sale)
                .Include(s => s.RealizedBy)
                .Where(a => a.Sale.Id == id)
                .ToListAsync();
        }

        public async Task<Abono> AddAbonoAsync(AddAbonoViewModel model, Entities.User user)
        {
            Abono abono = new();
            decimal sobra = model.Monto;
            var sales = await _context.Sales
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.Client.Id == model.IdClient
                        && s.IsCanceled == false
                )
                .ToListAsync();
            foreach (var item in sales)
            {
                if (sobra > 0)
                {
                    if (item.Saldo > sobra)
                    {
                        abono = new()
                        {
                            Sale = item,
                            Monto = sobra,
                            RealizedBy = user,
                            FechaAbono = DateTime.Now,
                            Store = await _context.Almacen.FirstOrDefaultAsync(
                                s => s.Id == model.IdStore
                            )
                        };

                        item.Saldo -= sobra;
                        sobra = 0;
                    }
                    else if (item.Saldo == sobra)
                    {
                        abono = new()
                        {
                            Sale = item,
                            Monto = sobra,
                            RealizedBy = user,
                            FechaAbono = DateTime.Now,
                            Store = await _context.Almacen.FirstOrDefaultAsync(
                                s => s.Id == model.IdStore
                            )
                        };
                        item.Saldo = 0;
                        item.IsCanceled = true;
                        sobra = 0;
                    }
                    else
                    {
                        abono = new()
                        {
                            Sale = item,
                            Monto = item.Saldo,
                            RealizedBy = user,
                            FechaAbono = DateTime.Now,
                            Store = await _context.Almacen.FirstOrDefaultAsync(
                                s => s.Id == model.IdStore
                            )
                        };

                        sobra -= item.Saldo;
                        item.Saldo = 0;
                        item.IsCanceled = true;
                    }
                    _context.Entry(item).State = EntityState.Modified;

                    _context.Abonos.Add(abono);
                }
            }

            await _context.SaveChangesAsync();
            return abono;
        }

        public async Task<Sales> AnularSaleAsync(int id, Entities.User user)
        {
            Sales sale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return sale;
            }
            var abono = await _context.Abonos.Where(a => a.Sale == sale).ToListAsync();
            foreach (var item in abono)
            {
                item.IsAnulado = true;
                _context.Entry(item).State = EntityState.Modified;
            }
            foreach (var item in sale.SaleDetails)
            {
                item.IsAnulado = true;
                item.AnulatedBy = user;
                item.FechaAnulacion = DateTime.Now;
                //Modificamos las existencias
                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == item.Product && e.Almacen == item.Store
                );

                existence.Existencia += item.Cantidad;
                _context.Entry(existence).State = EntityState.Modified;

                //Agregamos el Kardex de entrada al almacen destino
                var karList = await _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                    .ToListAsync();

                Kardex kar = karList.Where(k => k.Id == karList.Max(k => k.Id)).FirstOrDefault();

                Kardex kardex =
                    new()
                    {
                        Product = item.Product,
                        Fecha = DateTime.Now,
                        Concepto = "DEVOLUCION TOTAL DE VENTA",
                        Almacen = item.Store,
                        Entradas = item.Cantidad,
                        Salidas = 0,
                        Saldo = kar.Saldo + item.Cantidad,
                        User = user
                    };
                _context.Kardex.Add(kardex);
            }
            sale.IsAnulado = true;
            sale.Client.ContadorCompras -= 1;
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sales> AnularSaleParcialAsync(EditSaleViewModel model, Entities.User user)
        {
            Sales sale = await _context.Sales
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == model.IdSale);
            if (sale == null)
            {
                return sale;
            }
            foreach (var item in sale.SaleDetails)
            {
                int diferencia = 0;
                SaleDetail sd = model.SaleDetails.FirstOrDefault(s => s.Id == item.Id);
                if (sd == null)
                {
                    sale.ProductsCount -= 1;
                    item.IsAnulado = true;
                    item.AnulatedBy = user;
                    item.FechaAnulacion = DateTime.Now;

                    //hay que restarle a las existencias y agregar la salida al kardex
                    Existence existence = await _context.Existences.FirstOrDefaultAsync(
                        e => e.Producto == item.Product && e.Almacen == item.Store
                    );

                    existence.Existencia += item.Cantidad;
                    _context.Entry(existence).State = EntityState.Modified;

                    // Agregamos el Kardex de entrada al almacen destino
                    //Buscamos el ultimo reguistro de ese producto en el kardex
                    var karList = await _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                        .ToListAsync();

                    Kardex kar = karList
                        .Where(k => k.Id == karList.Max(k => k.Id))
                        .FirstOrDefault();

                    //creamos el objeto kardex
                    Kardex kardex =
                        new()
                        {
                            Product = item.Product,
                            Fecha = DateTime.Now,
                            Concepto = "DEVOLUCION PARCIAL DE VENTA",
                            Almacen = item.Store,
                            Entradas = item.Cantidad,
                            Salidas = 0,
                            Saldo = kar.Saldo + item.Cantidad,
                            User = user
                        };
                    _context.Kardex.Add(kardex);
                }
                else
                {
                    diferencia = item.Cantidad - sd.Cantidad;
                    item.Cantidad = sd.Cantidad;
                    item.CostoTotal = sd.CostoTotal;
                }
                // Modificamos las existencias
                //es es mayor que cero
                //hay que sumarle a las existencias y agregar la entrada al kardex
                if (diferencia > 0)
                {
                    //es es menor que cero
                    //hay que restarle a las existencias y agregar la salida al kardex
                    Existence existence = await _context.Existences.FirstOrDefaultAsync(
                        e => e.Producto == item.Product && e.Almacen == item.Store
                    );

                    existence.Existencia += diferencia;
                    _context.Entry(existence).State = EntityState.Modified;

                    // Agregamos el Kardex de entrada al almacen destino
                    //Buscamos el ultimo reguistro de ese producto en el kardex
                    var karList = await _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                        .ToListAsync();

                    Kardex kar = karList
                        .Where(k => k.Id == karList.Max(k => k.Id))
                        .FirstOrDefault();

                    //creamos el objeto kardex
                    Kardex kardex =
                        new()
                        {
                            Product = item.Product,
                            Fecha = DateTime.Now,
                            Concepto = "DEVOLUCION PARCIAL DE VENTA",
                            Almacen = item.Store,
                            Entradas = item.Cantidad,
                            Salidas = 0,
                            Saldo = kar.Saldo + item.Cantidad,
                            User = user
                        };
                    _context.Kardex.Add(kardex);
                }
            }

            sale.MontoVenta = model.Monto;
            if (!sale.IsCanceled)
            {
                sale.Saldo = model.Saldo;
            }
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<ICollection<GetSalesAndQuotesResponse>> GetSalesUncanceledByClientAsync(
            int idClient
        )
        {
            List<GetSalesAndQuotesResponse> result = new();
            var sales = await _context.Sales
                .Include(s => s.Store)
                .Where(
                    s => s.IsAnulado == false && s.Client.Id == idClient && s.IsCanceled == false
                )
                .ToListAsync();

            foreach (var item in sales)
            {
                GetSalesAndQuotesResponse temp =
                    new()
                    {
                        Sale = item,
                        Abonos = await _context.Abonos
                            .Include(a => a.RealizedBy)
                            .Where(a => a.Sale == item)
                            .ToListAsync()
                    };
                result.Add(temp);
            }

            return result;
        }

        public async Task<Abono> AddAbonoEspecificoAsync(
            AddAbonoEspecificoViewModel model,
            Entities.User user
        )
        {
            var sale = await _context.Sales
                .Include(s => s.Store)
                .FirstOrDefaultAsync(s => s.Id == model.IdSale);
            sale.Saldo -= model.Monto;
            if (sale.Saldo == 0)
            {
                sale.IsCanceled = true;
            }
            Abono abono =
                new()
                {
                    Sale = sale,
                    Monto = model.Monto,
                    RealizedBy = user,
                    FechaAbono = DateTime.Now,
                    Store = sale.Store
                };

            _context.Entry(sale).State = EntityState.Modified;
            _context.Abonos.Add(abono);
            await _context.SaveChangesAsync();
            return abono;
        }
    }
}

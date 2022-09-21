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
            DateTime hoy = DateTime.Now;
            Client cl = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient);
            if (cl != null)
            {
                cl.ContadorCompras += 1;
                if (!model.IsContado)
                {
                    cl.CreditoConsumido += model.MontoVenta;
                }
                _context.Entry(cl).State = EntityState.Modified;
            }

            Sales sale =
                new()
                {
                    IsEventual = model.IsEventual,
                    NombreCliente = model.IsEventual ? model.NombreCliente : "CLIENTE EVENTUAL",
                    Client = cl,
                    ProductsCount = model.SaleDetails.Count,
                    MontoVenta = model.MontoVenta,
                    IsDescuento = model.IsDescuento,
                    DescuentoXPercent = model.DescuentoXPercent,
                    DescuentoXMonto = model.DescuentoXMonto,
                    FechaVenta = hoy,
                    FacturedBy = user,
                    IsContado = model.IsContado,
                    IsCanceled = model.IsContado, //Si es de contado, esta cancelado
                    Saldo = model.IsContado ? 0 : model.MontoVenta,
                    FechaVencimiento = hoy.AddDays(15),
                    Store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.Storeid),
                    CodigoDescuento = model.CodigoDescuento,
                    MontoVentaAntesDescuento = model.MontoVentaAntesDescuento
                };

            if (model.IsContado)
            {
                var movList = await _context.CajaMovments
                    .Where(c => c.Store.Id == model.Storeid && c.CajaTipo.Id == 1)
                    .ToListAsync();

                var mov = movList.Where(m => m.Id == movList.Max(k => k.Id)).FirstOrDefault();
            }

            List<SaleDetail> detalles = new();
            foreach (var item in model.SaleDetails)
            {
                Producto prod = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );
                Almacen alm = await _context.Almacen.FirstOrDefaultAsync(
                    a => a.Id == item.Store.Id
                );

                //Modificamos las existencias
                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == prod && e.Almacen == alm
                );
                existence.Existencia -= item.Cantidad;
                existence.PrecioVentaDetalle = item.PVD;
                existence.PrecioVentaMayor = item.PVM;
                _context.Entry(existence).State = EntityState.Modified;

                //Se crea el objeto detalle venta
                SaleDetail saleDetail =
                    new()
                    {
                        Store = alm,
                        Product = prod,
                        Cantidad = item.Cantidad,
                        IsDescuento = item.IsDescuento,
                        DescuentoXPercent = item.DescuentoXPercent,
                        Descuento = item.Descuento,
                        CodigoDescuento = item.CodigoDescuento,
                        Ganancia = item.CostoTotal - (item.Cantidad * existence.PrecioCompra),
                        CostoUnitario = item.CostoUnitario,
                        PVM = item.PVM,
                        PVD = item.PVD,
                        CostoTotalAntesDescuento = item.CostoTotalAntesDescuento,
                        CostoTotalDespuesDescuento = item.CostoTotalDespuesDescuento,
                        CostoTotal = item.CostoTotal,
                        CostoCompra = existence.PrecioCompra,
                    };
                detalles.Add(saleDetail); //Se agrega a la lista

                //Agregamos el Kardex de entrada al almacen destino
                var karList = await _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                    .ToListAsync();

                Kardex kar = karList.Where(k => k.Id == karList.Max(k => k.Id)).FirstOrDefault();

                Kardex kardex =
                    new()
                    {
                        Product = prod,
                        Fecha = hoy,
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
                        FechaAbono = hoy,
                        Store = detalles[0].Store
                    };
                _context.Abonos.Add(abono);
            }

            //Unificamos objetos y los mandamos a la DB
            sale.SaleDetails = detalles;
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

            if (sale.IsContado == true)
            {
                CountAsientoContableDetails detalleDebito =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 66),
                        Debito = sale.MontoVenta,
                        Credito = 0
                    };
                countAsientoContableDetailsList.Add(detalleDebito);
            }
            else
            {
                CountAsientoContableDetails detalleDebito =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72),
                        Debito = sale.MontoVenta,
                        Credito = 0
                    };
                countAsientoContableDetailsList.Add(detalleDebito);
            }
            CountAsientoContableDetails detalleCredito =
                new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74),
                    Debito = 0,
                    Credito = sale.MontoVenta
                };
            countAsientoContableDetailsList.Add(detalleCredito);

            CountAsientoContable asientosContable =
                new()
                {
                    Fecha = sale.FechaVenta,
                    Referencia = $"VENTA DE PRODUCTOS SEGUN FACTURA: {sale.Id}",
                    LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
                    FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(
                        f => f.Id == 3
                    ),
                    Store = sale.Store,
                    User = user,
                    CountAsientoContableDetails = countAsientoContableDetailsList
                };
            _context.CountAsientosContables.Add(asientosContable);
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

        public async Task<ICollection<Abono>> AddAbonoAsync(
            AddAbonoViewModel model,
            Entities.User user
        )
        {
            DateTime hoy = DateTime.Now;
            Abono abono = new();
            List<Abono> abonoList = new();
            var movList = await _context.CajaMovments
                .Where(c => c.Store.Id == model.IdStore && c.CajaTipo.Id == 1)
                .ToListAsync();

            var mov = movList.Where(m => m.Id == movList.Max(k => k.Id)).FirstOrDefault();

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
                            FechaAbono = hoy,
                            Store = await _context.Almacen.FirstOrDefaultAsync(
                                s => s.Id == model.IdStore
                            )
                        };

                        item.Saldo -= sobra;
                        sobra = 0;
                        abonoList.Add(abono);
                    }
                    else if (item.Saldo == sobra)
                    {
                        abono = new()
                        {
                            Sale = item,
                            Monto = sobra,
                            RealizedBy = user,
                            FechaAbono = hoy,
                            Store = await _context.Almacen.FirstOrDefaultAsync(
                                s => s.Id == model.IdStore
                            )
                        };
                        item.Saldo = 0;
                        item.IsCanceled = true;
                        sobra = 0;
                        abonoList.Add(abono);
                    }
                    else
                    {
                        abono = new()
                        {
                            Sale = item,
                            Monto = item.Saldo,
                            RealizedBy = user,
                            FechaAbono = hoy,
                            Store = await _context.Almacen.FirstOrDefaultAsync(
                                s => s.Id == model.IdStore
                            )
                        };

                        abonoList.Add(abono);

                        sobra -= item.Saldo;
                        item.Saldo = 0;
                        item.IsCanceled = true;
                    }

                    _context.Entry(item).State = EntityState.Modified;
                    _context.Abonos.Add(abono);

                    List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

                    CountAsientoContableDetails detalleDebito =
                        new()
                        {
                            Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 66),
                            Debito = abono.Monto,
                            Credito = 0
                        };
                    countAsientoContableDetailsList.Add(detalleDebito);

                    CountAsientoContableDetails detalleCredito =
                        new()
                        {
                            Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72),
                            Debito = 0,
                            Credito = abono.Monto
                        };
                    countAsientoContableDetailsList.Add(detalleCredito);

                    CountAsientoContable asientosContable =
                        new()
                        {
                            Fecha = hoy,
                            Referencia =
                                $"ABONO POR VENTA DE PRODUCTOS SEGUN FACTURA: {abono.Sale.Id}",
                            LibroContable = await _context.CountLibros.FirstOrDefaultAsync(
                                c => c.Id == 4
                            ),
                            FuenteContable =
                                await _context.CountFuentesContables.FirstOrDefaultAsync(
                                    f => f.Id == 3
                                ),
                            Store = abono.Store,
                            User = user,
                            CountAsientoContableDetails = countAsientoContableDetailsList
                        };
                    _context.CountAsientosContables.Add(asientosContable);
                }
            }

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient);
            client.CreditoConsumido -= model.Monto;
            client.SaldoVencido -= model.Monto;
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return abonoList;
        }

        public async Task<Sales> AnularSaleAsync(int id, Entities.User user)
        {
            DateTime hoy = DateTime.Now;

            Sales sale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Include(s => s.Store)
                .FirstOrDefaultAsync(s => s.Id == id);

            var movList = await _context.CajaMovments
                .Where(c => c.Store.Id == sale.Store.Id && c.CajaTipo.Id == 1)
                .ToListAsync();

            var mov = movList.Where(m => m.Id == movList.Max(k => k.Id)).FirstOrDefault();

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

            List<SaleAnulationDetails> saleAnulationDetailList = new();
            foreach (var item in sale.SaleDetails)
            {
                item.IsAnulado = true;
                item.AnulatedBy = user;
                item.FechaAnulacion = hoy;

                SaleAnulationDetails saleAnulationDetail =
                    new()
                    {
                        FechaAnulacion = hoy,
                        CantidadAnulada = item.Cantidad,
                        SaleDetailAfectado = item
                    };

                saleAnulationDetailList.Add(saleAnulationDetail);

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
                        Fecha = hoy,
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
            sale.FechaAnulacion = hoy;
            sale.AnulatedBy = user;
            sale.Client.CreditoConsumido -= sale.MontoVenta;

            SaleAnulation saleAnulation =
                new()
                {
                    VentaAfectada = sale,
                    MontoAnulado = sale.MontoVenta,
                    FechaAnulacion = hoy,
                    AnulatedBy = user,
                    SaleAnulationDetails = saleAnulationDetailList,
                    Store = sale.Store
                };

            _context.SaleAnulations.Add(saleAnulation);

            if (!sale.IsEventual)
            {
                sale.Client.ContadorCompras -= 1;
                sale.Client.CreditoConsumido -= sale.MontoVenta;
                if (hoy.Date < sale.FechaVencimiento.Date)
                {
                    sale.Client.SaldoVencido -= sale.MontoVenta;
                    sale.Client.FacturasVencidas -= 1;
                }
            }
            _context.Entry(sale).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

            CountAsientoContableDetails detalleDebito =
                new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74),
                    Debito = sale.MontoVenta,
                    Credito = 0
                };
            countAsientoContableDetailsList.Add(detalleDebito);

            if (sale.IsContado == true)
            {
                CountAsientoContableDetails detalleCredito =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 66),
                        Debito = 0,
                        Credito = sale.MontoVenta
                    };
                countAsientoContableDetailsList.Add(detalleCredito);
            }
            else
            {
                CountAsientoContableDetails detalleCredito =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72),
                        Debito = 0,
                        Credito = sale.MontoVenta
                    };
                countAsientoContableDetailsList.Add(detalleCredito);
            }

            CountAsientoContable asientosContable =
                new()
                {
                    Fecha = sale.FechaVenta,
                    Referencia = $"DEVOLUCION SOBRE VENTA SEGUN FACTURA: {sale.Id}",
                    LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
                    FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(
                        f => f.Id == 3
                    ),
                    Store = sale.Store,
                    User = user,
                    CountAsientoContableDetails = countAsientoContableDetailsList
                };
            _context.CountAsientosContables.Add(asientosContable);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sales> AnularSaleParcialAsync(EditSaleViewModel model, Entities.User user)
        {
            DateTime hoy = DateTime.Now;

            Sales sale = await _context.Sales
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Include(s => s.SaleDetails)
                .Include(s => s.Store)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == model.IdSale);

            decimal salidaEfectivo = sale.MontoVenta - model.Monto;

            if (sale == null)
            {
                return sale;
            }

            List<SaleAnulationDetails> detalleList = new();
            decimal montoAnulado = 0;
            foreach (var item in sale.SaleDetails)
            {
                int diferencia = 0;
                SaleDetail sd = model.SaleDetails.FirstOrDefault(s => s.Id == item.Id);
                if (sd == null)
                {
                    sale.ProductsCount -= 1;
                    item.IsAnulado = true;
                    item.AnulatedBy = user;
                    item.FechaAnulacion = hoy;

                    SaleAnulationDetails detalleAnulation =
                        new()
                        {
                            FechaAnulacion = hoy,
                            CantidadAnulada = item.Cantidad,
                            SaleDetailAfectado = item
                        };
                    montoAnulado += item.CostoTotal;
                    detalleList.Add(detalleAnulation);

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
                            Fecha = hoy,
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
                    decimal diferenciaDinero = 0;
                    diferencia = item.Cantidad - sd.Cantidad;
                    diferenciaDinero = item.CostoTotal - sd.CostoTotal;
                    item.Cantidad = sd.Cantidad;
                    item.CostoTotal = sd.CostoTotal;
                    item.IsPartialAnulation = sd.IsPartialAnulation;
                    item.CantidadAnulada = sd.CantidadAnulada;

                    if (diferencia > 0)
                    {
                        SaleAnulationDetails detalleAnulation =
                            new()
                            {
                                FechaAnulacion = hoy,
                                CantidadAnulada = diferencia,
                                SaleDetailAfectado = item
                            };
                        montoAnulado += diferenciaDinero;
                        detalleList.Add(detalleAnulation);
                    }
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
                            Fecha = hoy,
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

            decimal diff = sale.MontoVenta - model.Monto;

            sale.MontoVenta = model.Monto;
            if (!sale.IsCanceled)
            {
                sale.Saldo = model.Saldo;
            }
            if (!sale.IsEventual)
            {
                sale.Client.CreditoConsumido -= montoAnulado;
                if (hoy.Date < sale.FechaVencimiento.Date)
                {
                    sale.Client.SaldoVencido -= sale.MontoVenta;
                    sale.Client.FacturasVencidas -= 1;
                }
            }

            _context.Entry(sale).State = EntityState.Modified;

            var movList = await _context.CajaMovments
                .Where(c => c.Store.Id == sale.Store.Id && c.CajaTipo.Id == 1)
                .ToListAsync();

            var mov = movList.Where(m => m.Id == movList.Max(k => k.Id)).FirstOrDefault();

            SaleAnulation newAnulation =
                new()
                {
                    VentaAfectada = sale,
                    MontoAnulado = montoAnulado,
                    FechaAnulacion = hoy,
                    AnulatedBy = user,
                    SaleAnulationDetails = detalleList,
                    Store = sale.Store
                };

            _context.SaleAnulations.Add(newAnulation);

            await _context.SaveChangesAsync();

            List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

            CountAsientoContableDetails detalleDebito =
                new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74),
                    Debito = diff,
                    Credito = 0
                };
            countAsientoContableDetailsList.Add(detalleDebito);

            if (sale.IsContado == true)
            {
                CountAsientoContableDetails detalleCredito =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 66),
                        Debito = 0,
                        Credito = diff
                    };
                countAsientoContableDetailsList.Add(detalleCredito);
            }
            else
            {
                CountAsientoContableDetails detalleCredito =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72),
                        Debito = 0,
                        Credito = diff
                    };
                countAsientoContableDetailsList.Add(detalleCredito);
            }

            CountAsientoContable asientosContable =
                new()
                {
                    Fecha = sale.FechaVenta,
                    Referencia = $"DEVOLUCION PARCIAL SOBRE VENTA SEGUN FACTURA: {sale.Id}",
                    LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
                    FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(
                        f => f.Id == 3
                    ),
                    Store = sale.Store,
                    User = user,
                    CountAsientoContableDetails = countAsientoContableDetailsList
                };
            _context.CountAsientosContables.Add(asientosContable);
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
                .Include(s => s.FacturedBy)
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
            DateTime hoy = DateTime.Now;

            var sale = await _context.Sales
                .Include(s => s.Store)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == model.IdSale);
            sale.Saldo -= model.Monto;
            sale.Client.CreditoConsumido -= model.Monto;
            sale.Client.SaldoVencido -= model.Monto;
            if (sale.Saldo == 0)
            {
                sale.IsCanceled = true;
                sale.Client.FacturasVencidas -= 1;
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

            //Agregando el registro contrable
            List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

            CountAsientoContableDetails detalleDebito =
                new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 66),
                    Debito = abono.Monto,
                    Credito = 0
                };
            countAsientoContableDetailsList.Add(detalleDebito);

            CountAsientoContableDetails detalleCredito =
                new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72),
                    Debito = 0,
                    Credito = abono.Monto
                };
            countAsientoContableDetailsList.Add(detalleCredito);

            CountAsientoContable asientosContable =
                new()
                {
                    Fecha = hoy,
                    Referencia = $"ABONO POR VENTA DE PRODUCTOS SEGUN FACTURA: {abono.Sale.Id}",
                    LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
                    FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(
                        f => f.Id == 3
                    ),
                    Store = abono.Store,
                    User = user,
                    CountAsientoContableDetails = countAsientoContableDetailsList
                };
            _context.CountAsientosContables.Add(asientosContable);
            await _context.SaveChangesAsync();
            return abono;
        }
    }
}

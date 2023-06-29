using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.FacturacionHelper
{
    public class FacturationHelper : IFacturationHelper
    {
        private readonly DataContext _context;

        public FacturationHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Facturacion> AddFacturacionAsync(
            AddFacturacionViewModel model,
            Entities.User user
        )
        {
            var store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.StoreId);
            Facturacion fact =
                new()
                {
                    IsEventual = model.IsEventual,
                    NombreCliente = model.NombreCliente,
                    Client = await _context.Clients.FirstOrDefaultAsync(
                        c => c.Id == model.ClientId
                    ),
                    ProductsCount = model.FacturacionDetails.Count,
                    MontoVenta = model.MontoVenta,
                    IsDescuento = model.IsDescuento,
                    DescuentoXMonto = model.DescuentoXMonto,
                    DescuentoXPercent = model.DescuentoXPercent,
                    MontoVentaAntesDescuento = model.MontoVentaAntesDescuento,
                    FechaVenta = DateTime.Now,
                    FacturedBy = user,
                    IsContado = model.IsContado,
                    IsCanceled = false,
                    IsAnulado = false,
                    Store = store,
                    CodigoDescuento = model.CodigoDescuento
                };

            List<FacturaDetails> facturaDetailList = new();

            foreach (var item in model.FacturacionDetails)
            {
                FacturaDetails facturaDetails =
                    new()
                    {
                        Store = store,
                        Product = await _context.Productos.FirstOrDefaultAsync(
                            p => p.Id == item.ProductId
                        ),
                        Cantidad = item.Cantidad,
                        IsDescuento = item.IsDescuento,
                        DescuentoXPercent = item.DescuentoXPercent,
                        Descuento = item.Descuento,
                        CodigoDescuento = item.CodigoDescuento,
                        CostoUnitario = item.CostoUnitario,
                        PVD = item.PVD,
                        PVM = item.PVM,
                        CostoTotalAntesDescuento = item.CostoTotalDespuesDescuento,
                        CostoTotalDespuesDescuento = item.CostoTotalDespuesDescuento,
                        CostoTotal = item.CostoTotal,
                        IsAnulado = item.IsAnulado,
                        CostoCompra = item.CostoCompra
                    };
                facturaDetailList.Add(facturaDetails);
            }

            fact.FacturaDetails = facturaDetailList;
            _context.Facturacions.Add(fact);
            await _context.SaveChangesAsync();
            return fact;
        }

        public async Task<Facturacion> DeleteFacturacionAsync(int factId, Entities.User user)
        {
            DateTime hoy = DateTime.Now;
            Facturacion fact = await _context.Facturacions
                .Include(f => f.FacturaDetails)
                .FirstOrDefaultAsync(f => f.Id == factId);
            fact.IsAnulado = true;
            fact.FechaAnulacion = hoy;
            fact.AnulatedBy = user;
            foreach (var item in fact.FacturaDetails)
            {
                item.IsAnulado = true;
                item.FechaAnulacion = hoy;
                item.AnulatedBy = user;
            }
            _context.Entry(fact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return fact;
        }

        public async Task<ICollection<Facturacion>> GetAnulatedFacturacionAsync(int storeId)
        {

            DateTime hoy = DateTime.Now;
            var result = await _context.Facturacions
                .Include(f => f.Client)
                .Where(f => f.IsCanceled == false && f.IsAnulado && f.FechaVenta.Year == hoy.Year && f.Store.Id == storeId )
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Facturacion>> GetCancelledFacturacionAsync(int storeId)
        {
            DateTime hoy = DateTime.Now;
            var result = await _context.Facturacions
                .Include(f => f.Client)
                .Include(f => f.Sale)
                .Where(f => f.IsCanceled && f.IsAnulado == false  && f.FechaVenta.Year == hoy.Year && f.Store.Id == storeId)
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Facturacion>> GetFacturacionAsync(int storeId)
        {
            DateTime hoy = DateTime.Now;

            var result = await _context.Facturacions
                .Include(f => f.Client)
                .Include(f => f.FacturaDetails)
                .Where(f => f.IsCanceled == false && f.FechaVenta.Year == hoy.Year && f.IsAnulado == false && f.Store.Id == storeId)
                .ToListAsync();

            return result;
        }

        public async Task<Sales> GetReprintBillAsync(int saleId)
        {
            var sale = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);
            return sale;
        }

        public async Task<ICollection<TipoPago>> GetTipoPagoAsync()
        {
            return await _context.TipoPagos.ToListAsync();
        }

        public async Task<Sales> PayFacturaAsync(PayFactViewModel model, Entities.User user)
        {
            Facturacion fact = await _context.Facturacions
                .Include(f => f.Client)
                .Include(f => f.FacturedBy)
                .Include(f => f.FacturaDetails)
                .ThenInclude(fd => fd.Product)
                .Include(f => f.FacturaDetails)
                .ThenInclude(fd => fd.Store)
                .FirstOrDefaultAsync(f => f.Id == model.FacturaId);
            fact.IsCanceled = true;
            fact.IsDescuento = model.IsDescuento;
            fact.DescuentoXPercent = model.DescuentoXPercent;
            fact.DescuentoXMonto = model.DescuentoXMonto;
            fact.CodigoDescuento = model.CodigoDescuento;
            fact.MontoVenta = model.MontoVenta;
            fact.MontoVentaAntesDescuento = model.MontoVentaAntesDescuento;
            fact.PaidBy = user;

            DateTime hoy = DateTime.Now;

            if (fact.Client != null)
            {
                fact.Client.ContadorCompras += 1;
            }

            TipoPago tp = await _context.TipoPagos.FirstOrDefaultAsync(
                t => t.Id == model.TipoPagoId
            );

            Sales sale =
                new()
                {
                    IsEventual = fact.IsEventual,
                    NombreCliente = fact.IsEventual ? fact.NombreCliente : "CLIENTE EVENTUAL",
                    Client = fact.Client,
                    ProductsCount = fact.FacturaDetails.Count,
                    MontoVenta = fact.MontoVenta,
                    IsDescuento = fact.IsDescuento,
                    DescuentoXPercent = fact.DescuentoXPercent,
                    DescuentoXMonto = fact.DescuentoXMonto,
                    FechaVenta = hoy,
                    FacturedBy = fact.FacturedBy,
                    IsContado = fact.IsContado,
                    IsCanceled = fact.IsContado, //Si es de contado, esta cancelado
                    Saldo = fact.IsContado ? 0 : fact.MontoVenta,
                    FechaVencimiento = hoy.AddDays(15),
                    Store = fact.Store,
                    CodigoDescuento = fact.CodigoDescuento,
                    MontoVentaAntesDescuento = fact.MontoVentaAntesDescuento,
                    TipoPago = tp,
                    Reference = model.Reference
                };

            List<SaleDetail> detalles = new();
            foreach (var item in fact.FacturaDetails)
            {
                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == item.Product && e.Almacen == item.Store
                );
                existence.Existencia -= item.Cantidad;
                existence.PrecioVentaDetalle = item.PVD;
                existence.PrecioVentaMayor = item.PVM;
                _context.Entry(existence).State = EntityState.Modified;

                //Se crea el objeto detalle venta
                SaleDetail saleDetail =
                    new()
                    {
                        Store = item.Store,
                        Product = item.Product,
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
                        CostoCompra = item.CostoCompra
                    };
                detalles.Add(saleDetail); //Se agrega a la lista

                //Agregamos el Kardex de entrada al almacen destino
                Kardex kar = await _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                    .OrderByDescending(k => k.Id)
                    .FirstOrDefaultAsync();

                int saldo = kar == null ? 0 : kar.Saldo;

                Kardex kardex =
                    new()
                    {
                        Product = item.Product,
                        Fecha = hoy,
                        Concepto = fact.IsContado ? "VENTA DE CONTADO" : "VENTA DE CREDITO",
                        Almacen = item.Store,
                        Entradas = 0,
                        Salidas = item.Cantidad,
                        Saldo = saldo - item.Cantidad,
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
                        Store = detalles[0].Store,
                        TipoPago = tp,
                        Reference = model.Reference
                    };
                _context.Abonos.Add(abono);
            }

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
            fact.Sale = sale;
            _context.Entry(fact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return sale;
        }
    }
}

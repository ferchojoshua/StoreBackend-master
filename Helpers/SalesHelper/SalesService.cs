using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
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

        public async Task<ICollection<Sales>> GetSalesListAsync()
        {
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado == false)
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
            Sales sale =
                new()
                {
                    IsEventual = model.IsEventual,
                    NombreCliente = model.IsEventual ? model.NombreCliente : "",
                    Client = await _context.Clients.FirstOrDefaultAsync(
                        c => c.Id == model.IdClient
                    ),
                    ProductsCount = model.SaleDetails.Count,
                    MontoVenta = model.MontoVenta,
                    FechaVenta = DateTime.Now,
                    FacturedBy = user,
                    IsContado = model.IsContado,
                    IsCanceled = model.IsContado, //Si es de contado, esta cancelado
                    Saldo = model.IsContado ? 0 : model.MontoVenta,
                    FechaVencimiento = DateTime.Now.AddDays(15)
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
            Sales sale = await _context.Sales.FirstOrDefaultAsync(s => s.Id == model.IdSale);
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
                    FechaAbono = DateTime.Now
                };
            _context.Entry(sale).State = EntityState.Modified;
            _context.Abonos.Add(abono);
            await _context.SaveChangesAsync();
            return abono;
        }

        public async Task<Sales> AnularSaleAsync(int id, Entities.User user)
        {
            Sales sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return sale;
            }
            foreach (var item in sale.SaleDetails)
            {
                item.IsAnulado = true;

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
            _context.Entry(sale).State = EntityState.Modified;

            AnuladaSales anuladaSales =
                new()
                {
                    Sale = sale,
                    FechaAnulacion = DateTime.Now,
                    AnuledBy = user
                };

            _context.AnuladaSales.Add(anuladaSales);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<Sales> AnularSaleParcialAsync(int id, int productId, Entities.User user)
        {
            Sales sale = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return sale;
            }

            foreach (var item in sale.SaleDetails)
            {
                if (item.Product.Id == productId)
                {
                    item.IsAnulado = true;

                    sale.MontoVenta -= item.CostoTotal;

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

                    Kardex kar = karList
                        .Where(k => k.Id == karList.Max(k => k.Id))
                        .FirstOrDefault();

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
            }

            return sale;
        }
    }
}

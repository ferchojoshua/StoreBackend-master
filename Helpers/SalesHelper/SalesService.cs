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
                    e => e.Producto == prod
                );
                existence.Almacen = alm;
                existence.Producto = prod;
                existence.Existencia -= saleDetail.Cantidad;
                existence.PrecioVentaDetalle = item.PVD;
                existence.PrecioVentaMayor = item.PVM;
                _context.Entry(existence).State = EntityState.Modified;

                //Modificamos el Kardex
                int totalEntradas = _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == alm)
                    .Sum(k => k.Entradas);

                int totaSalidas = _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == alm)
                    .Sum(k => k.Salidas);

                int saldo = totalEntradas - totaSalidas;

                Kardex kardex =
                    new()
                    {
                        Product = prod,
                        Fecha = DateTime.Now,
                        Concepto = model.IsContado ? "VENTA DE CONTADO" : "VENTA DE CREDITO",
                        Almacen = alm,
                        Entradas = 0,
                        Salidas = item.Cantidad,
                        Saldo = saldo - item.Cantidad,
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
    }
}

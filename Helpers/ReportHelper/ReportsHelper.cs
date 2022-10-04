using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.ReportHelper
{
    public class ReportsHelper : IReportsHelper
    {
        private readonly DataContext _context;

        public ReportsHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Sales>> ReportMasterVentas(MasterVentasViewModel model)
        {
            List<Sales> result = new();
            if (model.StoreId == 0)
            {
                if (model.ContadoSales && model.CreditSales)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.SaleDetails)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                        )
                        .ToListAsync();
                    return result;
                }
                else if (model.ContadoSales)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.SaleDetails)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.IsContado
                        )
                        .ToListAsync();
                    return result;
                }
                else
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.SaleDetails)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.IsContado == false
                        )
                        .ToListAsync();
                    return result;
                }
            }
            else
            {
                if (model.ContadoSales && model.CreditSales)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.SaleDetails)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.Store.Id == model.StoreId
                        )
                        .ToListAsync();
                    return result;
                }
                else if (model.ContadoSales)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.Store)
                        .Include(s => s.SaleDetails)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.Store.Id == model.StoreId
                                && s.IsContado
                        )
                        .ToListAsync();
                    return result;
                }
                else
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.SaleDetails)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.Store.Id == model.StoreId
                                && s.IsContado == false
                        )
                        .ToListAsync();
                    return result;
                }
            }
        }

        public async Task<ICollection<Sales>> ReportCuentasXCobrar(CuentasXCobrarViewModel model)
        {
            List<Sales> result = new();
            if (model.StoreId == 0)
            {
                if (model.ClientId == 0)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date.Date >= model.Desde.Date.Date
                                && s.FechaVenta.Date.Date <= model.Hasta.Date.Date
                                && s.IsContado == false
                        )
                        .ToListAsync();
                    return result;
                }
                else
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.IsContado == false
                                && s.Client.Id == model.ClientId
                        )
                        .ToListAsync();
                    return result;
                }
            }
            else
            {
                if (model.ClientId == 0)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.Store.Id == model.StoreId
                                && s.IsContado == false
                        )
                        .ToListAsync();
                    return result;
                }
                else
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date >= model.Desde.Date
                                && s.FechaVenta.Date <= model.Hasta.Date
                                && s.Store.Id == model.StoreId
                                && s.IsContado == false
                                && s.Client.Id == model.ClientId
                        )
                        .ToListAsync();
                    return result;
                }
            }
        }

        public async Task<ICollection<ReportResponse>> ReportArticulosVendidos(
            ArtVendidosViewModel model
        )
        {
            List<ReportResponse> result = new();

            List<Sales> sales = new();

            if (model.StoreId != 0)
            {
                if (model.ClientId != 0)
                {
                    if (model.IncludeUncanceledSales)
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.Store.Id == model.StoreId
                                    && s.Client.Id == model.ClientId
                            )
                            .ToListAsync();
                    }
                    else
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.Store.Id == model.StoreId
                                    && s.Client.Id == model.ClientId
                                    && s.IsCanceled
                            )
                            .ToListAsync();
                    }
                }
                else
                {
                    if (model.IncludeUncanceledSales)
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.Store.Id == model.StoreId
                            )
                            .ToListAsync();
                    }
                    else
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.Store.Id == model.StoreId
                                    && s.IsCanceled
                            )
                            .ToListAsync();
                    }
                }
            }
            else
            {
                if (model.ClientId != 0)
                {
                    if (model.IncludeUncanceledSales)
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.Client.Id == model.ClientId
                            )
                            .ToListAsync();
                    }
                    else
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.Client.Id == model.ClientId
                                    && s.IsCanceled
                            )
                            .ToListAsync();
                    }
                }
                else
                {
                    if (model.IncludeUncanceledSales)
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                            )
                            .ToListAsync();
                    }
                    else
                    {
                        sales = await _context.Sales
                            .Include(s => s.Client)
                            .Include(s => s.Store)
                            .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                            .ThenInclude(sd => sd.Product)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.TipoNegocio)
                            .Include(s => s.SaleDetails)
                            .ThenInclude(sd => sd.Product.Familia)
                            .Where(
                                s =>
                                    s.IsAnulado == false
                                    && s.FechaVenta.Date >= model.Desde.Date
                                    && s.FechaVenta.Date <= model.Hasta.Date
                                    && s.IsCanceled
                            )
                            .ToListAsync();
                    }
                }
            }

            List<SaleDetail> detailList = new();
            foreach (var sale in sales)
            {
                foreach (var item in sale.SaleDetails)
                {
                    detailList.Add(item);
                }
            }

            ReportResponse r = new();
            if (model.TipoNegocioId != 0)
            {
                if (model.FamiliaId != 0)
                {
                    var vProd = detailList
                        .Where(
                            p =>
                                p.Product.TipoNegocio.Id == model.TipoNegocioId
                                && (
                                    p.Product.Familia == null
                                    || p.Product.Familia.Id == model.FamiliaId
                                )
                        )
                        .GroupBy(sd => sd.Product)
                        .Select(
                            sd =>
                                new
                                {
                                    sd.Key.BarCode,
                                    sd.Key.Description,
                                    CantidadVendida = sd.Sum(x => x.Cantidad),
                                    CostoCompra = sd.Sum(x => x.CostoTotal - x.Ganancia),
                                    MontoVenta = sd.Sum(x => x.CostoTotal),
                                    Utilidad = sd.Sum(x => x.Ganancia)
                                }
                        );
                    foreach (var item in vProd)
                    {
                        r = new()
                        {
                            BarCode = item.BarCode,
                            Producto = item.Description,
                            CantidadVendida = item.CantidadVendida,
                            CostoCompra = item.CostoCompra,
                            MontoVenta = item.MontoVenta,
                            Utilidad = item.Utilidad
                        };
                        result.Add(r);
                    }
                }
                else
                {
                    var vProd = detailList
                        .Where(p => p.Product.TipoNegocio.Id == model.TipoNegocioId)
                        .GroupBy(sd => sd.Product)
                        .Select(
                            sd =>
                                new
                                {
                                    sd.Key.BarCode,
                                    sd.Key.Description,
                                    CantidadVendida = sd.Sum(x => x.Cantidad),
                                    CostoCompra = sd.Sum(x => x.CostoTotal - x.Ganancia),
                                    MontoVenta = sd.Sum(x => x.CostoTotal),
                                    Utilidad = sd.Sum(x => x.Ganancia)
                                }
                        );
                    foreach (var item in vProd)
                    {
                        r = new()
                        {
                            BarCode = item.BarCode,
                            Producto = item.Description,
                            CantidadVendida = item.CantidadVendida,
                            CostoCompra = item.CostoCompra,
                            MontoVenta = item.MontoVenta,
                            Utilidad = item.Utilidad
                        };
                        result.Add(r);
                    }
                }
            }
            else
            {
                if (model.FamiliaId != 0)
                {
                    var vProd = detailList
                        .Where(p => p.Product.Familia.Id == model.FamiliaId)
                        .GroupBy(sd => sd.Product)
                        .Select(
                            sd =>
                                new
                                {
                                    sd.Key.BarCode,
                                    sd.Key.Description,
                                    CantidadVendida = sd.Sum(x => x.Cantidad),
                                    CostoCompra = sd.Sum(x => x.CostoTotal - x.Ganancia),
                                    MontoVenta = sd.Sum(x => x.CostoTotal),
                                    Utilidad = sd.Sum(x => x.Ganancia)
                                }
                        );
                    foreach (var item in vProd)
                    {
                        r = new()
                        {
                            BarCode = item.BarCode,
                            Producto = item.Description,
                            CantidadVendida = item.CantidadVendida,
                            CostoCompra = item.CostoCompra,
                            MontoVenta = item.MontoVenta,
                            Utilidad = item.Utilidad
                        };
                        result.Add(r);
                    }
                }
                else
                {
                    var vProd = detailList
                        .GroupBy(sd => sd.Product)
                        .Select(
                            sd =>
                                new
                                {
                                    sd.Key.BarCode,
                                    sd.Key.Description,
                                    CantidadVendida = sd.Sum(x => x.Cantidad),
                                    CostoCompra = sd.Sum(x => x.CostoTotal - x.Ganancia),
                                    MontoVenta = sd.Sum(x => x.CostoTotal),
                                    Utilidad = sd.Sum(x => x.Ganancia)
                                }
                        );
                    foreach (var item in vProd)
                    {
                        r = new()
                        {
                            BarCode = item.BarCode,
                            Producto = item.Description,
                            CantidadVendida = item.CantidadVendida,
                            CostoCompra = item.CostoCompra,
                            MontoVenta = item.MontoVenta,
                            Utilidad = item.Utilidad
                        };
                        result.Add(r);
                    }
                }
            }

            return result;
        }

        public async Task<DailyCloseResponse> ReportCierreDiario(CierreDiarioViewModel model)
        {
            DailyCloseResponse result = new();
            DateTime fechaHoraDesde = DateTime.Parse(model.Desde);
            DateTime fechaHoraHasta = DateTime.Parse(model.Hasta);
            if (model.StoreId != 0)
            {
                var salesByStore = await _context.Sales
                    .Include(s => s.Client)
                    .Include(s => s.SaleDetails)
                    .Where(
                        s =>
                            s.Store.Id == model.StoreId
                            && s.FechaVenta >= fechaHoraDesde
                            && s.FechaVenta <= fechaHoraHasta
                    // && s.IsAnulado == false
                    )
                    .ToListAsync();

                var devolucionesByStore = await _context.SaleAnulations
                    .Include(s => s.VentaAfectada)
                    .ThenInclude(s => s.Client)
                    .Include(s => s.AnulatedBy)
                    .Include(s => s.Store)
                    .Where(
                        s =>
                            s.Store.Id == model.StoreId
                            && s.FechaAnulacion >= fechaHoraDesde
                            && s.FechaAnulacion <= fechaHoraHasta
                    )
                    .ToListAsync();

                var abonoByStore = await _context.Abonos
                    .Include(a => a.Sale)
                    .ThenInclude(s => s.Client)
                    .Where(
                        a =>
                            a.Sale.IsContado == false
                            && a.IsAnulado == false
                            && a.Store.Id == model.StoreId
                            && a.FechaAbono >= fechaHoraDesde
                            && a.FechaAbono <= fechaHoraHasta
                    )
                    .ToListAsync();

                result.SaleList = salesByStore;
                result.AnulatedSaleList = devolucionesByStore;
                result.AbonoList = abonoByStore;

                return result;
            }

            var sales = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .Where(
                    s => s.FechaVenta >= fechaHoraDesde && s.FechaVenta <= fechaHoraHasta
                // && s.IsAnulado == false
                )
                .ToListAsync();

            var devoluciones = await _context.SaleAnulations
                .Include(s => s.VentaAfectada)
                .ThenInclude(s => s.Client)
                .Include(s => s.AnulatedBy)
                .Include(s => s.Store)
                .Where(
                    s => s.FechaAnulacion >= fechaHoraDesde && s.FechaAnulacion <= fechaHoraHasta
                )
                .ToListAsync();

            var abono = await _context.Abonos
                .Include(a => a.Sale)
                .ThenInclude(s => s.Client)
                .Where(
                    a =>
                        a.Sale.IsContado == false
                        && a.IsAnulado == false
                        && a.FechaAbono >= fechaHoraDesde
                        && a.FechaAbono <= fechaHoraHasta
                        && a.Sale.IsContado == false
                )
                .ToListAsync();

            result.SaleList = sales;
            result.AnulatedSaleList = devoluciones;
            result.AbonoList = abono;
            return result;
        }

        public async Task<ICollection<CajaMovment>> ReportCajaChica(CajaChicaViewModel model)
        {
            if (model.StoreId != 0)
            {
                var cajaMovsById = await _context.CajaMovments
                    .Where(
                        c =>
                            c.Fecha.Date >= model.Desde.Date
                            && c.Fecha.Date <= model.Hasta.Date
                            && c.Store.Id == model.StoreId
                            && c.CajaTipo.Id == 1
                    )
                    .ToListAsync();
                return cajaMovsById;
            }

            var cajaMovs = await _context.CajaMovments
                .Where(
                    c =>
                        c.Fecha.Date >= model.Desde.Date
                        && c.Fecha.Date <= model.Hasta.Date
                        && c.CajaTipo.Id == 1
                )
                .ToListAsync();
            return cajaMovs;
        }

        public async Task<ICollection<Existence>> ReportArticulosNoVendidos(
            ArtNoVendidosViewModel model
        )
        {
            List<Sales> sales = new();
            if (model.StoreId != 0)
            {
                sales = await _context.Sales
                    .Include(s => s.Store)
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product.TipoNegocio)
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product.Familia)
                    .Where(
                        s =>
                            s.IsAnulado == false
                            && s.FechaVenta.Date >= model.Desde.Date
                            && s.FechaVenta.Date <= model.Hasta.Date
                            && s.Store.Id == model.StoreId
                    )
                    .ToListAsync();
            }
            else
            {
                sales = await _context.Sales
                    .Include(s => s.Store)
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product)
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product.TipoNegocio)
                    .Include(s => s.SaleDetails)
                    .ThenInclude(sd => sd.Product.Familia)
                    .Where(
                        s =>
                            s.IsAnulado == false
                            && s.FechaVenta.Date >= model.Desde.Date
                            && s.FechaVenta.Date <= model.Hasta.Date
                    )
                    .ToListAsync();
            }

            List<string> detailList = new();
            foreach (var sale in sales)
            {
                foreach (var item in sale.SaleDetails)
                {
                    detailList.Add(item.Store.Id.ToString() + item.Product.Id.ToString());
                }
            }

            HashSet<string> hasNoDuplicates = new(detailList);
            List<string> detailListNoDuplicates = hasNoDuplicates.ToList();

            ProdNoVendidosResponse r = new();

            if (model.TipoNegocioId != 0)
            {
                if (model.FamiliaId != 0)
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                && e.Producto.TipoNegocio.Id == model.TipoNegocioId
                                && e.Producto.Familia.Id == model.FamiliaId
                                && !detailListNoDuplicates.Contains(
                                    e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto)
                        // .OrderBy(e => e.Almacen)
                        .ToListAsync();
                    return vExistences;
                }
                else
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                && e.Producto.TipoNegocio.Id == model.TipoNegocioId
                                && !detailListNoDuplicates.Contains(
                                    e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto)
                        // .OrderBy(e => e.Almacen)
                        .ToListAsync();
                    return vExistences;
                }
            }
            else
            {
                if (model.FamiliaId != 0)
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                && e.Producto.Familia.Id == model.FamiliaId
                                && !detailListNoDuplicates.Contains(
                                    e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto)
                        // .OrderBy(e => e.Almacen)
                        .ToListAsync();
                    return vExistences;
                }
                else
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                && !detailListNoDuplicates.Contains(
                                    e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto)
                        // .OrderBy(e => e.Almacen)
                        .ToListAsync();
                    return vExistences;
                }
            }
        }

        public async Task<ICollection<Abono>> ReportIngresos(IngresosViewModel model)
        {
            if (model.StoreId != 0)
            {
                var ingresosByStore = await _context.Abonos
                    .Include(a => a.Sale)
                    .ThenInclude(s => s.Client)
                    .Where(
                        s =>
                            s.IsAnulado == false
                            && s.FechaAbono >= model.Desde
                            && s.FechaAbono <= model.Hasta
                            && s.Store.Id == model.StoreId
                            && s.IsAnulado == false
                    )
                    .ToListAsync();
                return ingresosByStore;
            }
            var ingresos = await _context.Abonos
                .Include(a => a.Sale)
                .ThenInclude(s => s.Client)
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.FechaAbono >= model.Desde
                        && s.FechaAbono <= model.Hasta
                        && s.IsAnulado == false
                )
                .ToListAsync();
            return ingresos;
        }

        public async Task<ICollection<ProductIn>> ReportCompras(ComprasViewModel model)
        {
            if (model.ContadoCompras && model.CreditCompras)
            {
                return await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Where(pi => pi.FechaIngreso >= model.Desde && pi.FechaIngreso <= model.Hasta)
                    .ToListAsync();
            }
            else if (model.ContadoCompras && model.CreditCompras == false)
            {
                return await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Where(
                        pi =>
                            pi.FechaIngreso >= model.Desde
                            && pi.FechaIngreso <= model.Hasta
                            && pi.IsCanceled == true
                    )
                    .ToListAsync();
            }
            else
            {
                return await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Where(
                        pi =>
                            pi.FechaIngreso >= model.Desde
                            && pi.FechaIngreso <= model.Hasta
                            && pi.IsCanceled == true
                    )
                    .ToListAsync();
            }
        }
    }
}

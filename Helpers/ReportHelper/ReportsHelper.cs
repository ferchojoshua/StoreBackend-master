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
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
                        )
                        .ToListAsync();
                    return result;
                }
                else if (model.ContadoSales)
                {
                    result = await _context.Sales
                        .Include(s => s.Client)
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
                                && s.IsContado
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                        .Include(s => s.Store)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                    sales = await _context.Sales
                        .Include(s => s.Client)
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
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
                        .Include(s => s.SaleDetails)
                        .ThenInclude(sd => sd.Product)
                        .Include(s => s.SaleDetails)
                        .ThenInclude(sd => sd.Product.TipoNegocio)
                        .Include(s => s.SaleDetails)
                        .ThenInclude(sd => sd.Product.Familia)
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
                                && s.Store.Id == model.StoreId
                        )
                        .ToListAsync();
                }
            }
            else
            {
                if (model.ClientId != 0)
                {
                    sales = await _context.Sales
                        .Include(s => s.Client)
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
                                && s.Client.Id == model.ClientId
                        )
                        .ToListAsync();
                }
                else
                {
                    sales = await _context.Sales
                        .Include(s => s.Client)
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
                                && s.FechaVenta >= model.Desde
                                && s.FechaVenta <= model.Hasta
                        )
                        .ToListAsync();
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
    }
}

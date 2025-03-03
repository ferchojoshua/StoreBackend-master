using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;
using System;
using System.Data;

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



        public async Task<ICollection<ReportResponse>> ReportArticulosVendidos(ArtVendidosViewModel model
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
                                    Descuento = sd.Sum(x => x.Descuento),
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
                            Descuento = item.Descuento,
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
                                    Descuento = sd.Sum(x => x.Descuento),
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
                            Descuento = item.Descuento,
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
                                    Descuento = sd.Sum(x => x.Descuento),
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
                            Descuento = item.Descuento,
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
                                    Descuento = sd.Sum(x => x.Descuento),
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
                            Descuento = item.Descuento,
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
                // Ventas normales
                var salesByStore = await _context.Sales
                    .Include(s => s.Client)
                    .Include(s => s.SaleDetails)
                    .Where(
                        s =>
                            s.Store.Id == model.StoreId
                            && s.FechaVenta >= fechaHoraDesde
                            && s.FechaVenta <= fechaHoraHasta
                            && s.IsAnulado == false  
                            //&& s.IsContado == true   
                     )
                    .ToListAsync();
                // Anulaciones
                var anulacionesByStore = await _context.Sales
                    .Include(s => s.Client)
                    .Include(s => s.SaleDetails)
                    .Include(s => s.Store)
                    .Where(s =>
                        s.Store.Id == model.StoreId &&
                        s.IsSaleCancelled == true &&
                        s.FechaAnulacion >= fechaHoraDesde &&
                        s.FechaAnulacion <= fechaHoraHasta
                    )
                    .ToListAsync();
                // Devoluciones

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

                 // Abonos
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
                result.AnulatedforIdSaleList = anulacionesByStore;
                result.AnulatedSaleList = devolucionesByStore;
                result.AbonoList = abonoByStore;
                return result;
            }


            // Ventas normales (todas las tiendas)
            var sales = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .Where(
                    s => s.FechaVenta >= fechaHoraDesde 
                   && s.FechaVenta <= fechaHoraHasta
                   && s.IsAnulado == false
                   
                )
                .ToListAsync();

                 // Anulaciones (todas las tiendas)
                var anulaciones = await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.SaleDetails)
                .Include(s => s.Store)
                .Where(s =>
                    s.IsSaleCancelled == true &&
                    s.FechaAnulacion >= fechaHoraDesde &&
                    s.FechaAnulacion <= fechaHoraHasta
                )
                .ToListAsync();
            // Devoluciones (todas las tiendas)
            var devoluciones = await _context.SaleAnulations
                .Include(s => s.VentaAfectada)
                .ThenInclude(s => s.Client)
                .Include(s => s.AnulatedBy)
                .Include(s => s.Store)
                .Where(
                    s => s.FechaAnulacion >= fechaHoraDesde && s.FechaAnulacion <= fechaHoraHasta
                )
                .ToListAsync();

            // Abonos (todas las tiendas)
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
            result.AnulatedforIdSaleList = anulaciones;
            result.AnulatedSaleList = devoluciones;
            result.AbonoList = abono;
           return result;
        }

          public async Task<ICollection<CajaMovment>> ReportCajaChica(CajaChicaViewModel model)
        {
            if (model.StoreId != 0)
            {
                var cajaMovsById = await _context.CajaMovments
                     .Include(c => c.Store)
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
                .Include(c => c.Store)
                .Where(
                    c =>
                        c.Fecha.Date >= model.Desde.Date
                        && c.Fecha.Date <= model.Hasta.Date
                        && c.CajaTipo.Id == 1
                )
                .ToListAsync();
            return cajaMovs;
        }

        public async Task<ICollection<Existence>> ReportArticulosNoVendidos(ArtNoVendidosViewModel model)
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
                    .Where(s => 
                            s.IsAnulado == false
                            && s.FechaVenta.Date >= model.Desde.Date
                            && s.FechaVenta.Date <= model.Hasta.Date
                            && s.Store.Id == model.StoreId
                    )
                    .ToListAsync();
            }
            else if (model.StoreId == 0 && model.TipoNegocioId != 0)
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
                            && s.Store.Id >= model.StoreId

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
                             && s.Store.Id >= model.StoreId 
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

            if (model.StoreId != 0)
            {
                if (model.FamiliaId != 0 && model.TipoNegocioId != 0 && model.StoreId != 0)
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
                                && e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto).ThenBy(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 03/06/2023
                        .ToListAsync();
                    return vExistences;
                }
                else if (model.FamiliaId == 0 && model.TipoNegocioId != 0 && model.StoreId != 0)
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
                                && e.Producto.Familia.Id >= model.FamiliaId
                                && e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto).ThenBy(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 03/06/2023
                        .ToListAsync();
                    return vExistences;
                }
                else if (model.FamiliaId != 0 && model.TipoNegocioId == 0 && model.StoreId != 0)
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                && e.Producto.TipoNegocio.Id >= model.TipoNegocioId
                                && e.Producto.Familia.Id == model.FamiliaId
                                && e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto).ThenBy(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 03/06/2023
                        .ToListAsync();
                    return vExistences;
                }
                else if (model.FamiliaId == 0 && model.TipoNegocioId == 0 && model.StoreId != 0)
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                && e.Producto.TipoNegocio.Id >= model.TipoNegocioId
                                && e.Producto.Familia.Id >= model.FamiliaId
                                && e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString()
                                )
                        )
                        .OrderBy(e => e.Producto).ThenBy(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 03/06/2023
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
                                 && e.Producto.Familia.Id >= model.FamiliaId
                                && e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString()                          
                                )
                        )
                        //.OrderBy(e => e.Producto)
                        .OrderBy(e => e.Producto).ThenBy(e => e.Almacen.Id)
                        // .OrderBy(e => e.Almacen)
                        .ToListAsync();
                    return vExistences;
                }
            }
            else
            {
                 if (model.FamiliaId != 0 && model.StoreId != 0 && model.TipoNegocioId != 0)
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
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString())
                                 && e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                        )
                        .OrderBy(e => e.Producto).OrderByDescending(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 09/06/2023
                                                                                    
                        .ToListAsync();
                    return vExistences;
                }
                else if (model.FamiliaId == 0 && model.StoreId != 0 && model.TipoNegocioId != 0)
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                 && e.Producto.Familia.Id >= model.FamiliaId
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString())
                                && e.Almacen.Id >= model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023

                                )
                        .OrderBy(e => e.Producto).OrderByDescending(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 09/06/2023
                        .ToListAsync();
                    return vExistences;
                }
                else if (model.FamiliaId == 0 && model.StoreId == 0 && model.TipoNegocioId != 0)
                {
                    var vExistences = await _context.Existences
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.TipoNegocio)
                        .Include(e => e.Producto)
                        .ThenInclude(p => p.Familia)
                        .Where(
                            e =>
                                e.Existencia > 0
                                 && e.Producto.Familia.Id >= model.FamiliaId
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString())
                                && e.Almacen.Id >= model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && e.Producto.TipoNegocio.Id == model.TipoNegocioId

                                )
                        .OrderBy(e => e.Producto).OrderByDescending(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 09/06/2023
                        .ToListAsync();
                    return vExistences;
                }
                else if (model.FamiliaId != 0 && model.StoreId == 0 && model.TipoNegocioId == 0)
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
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString())
                                && e.Almacen.Id >= model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                && e.Producto.TipoNegocio.Id >= model.TipoNegocioId

                                )
                        .OrderBy(e => e.Producto).OrderByDescending(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 09/06/2023
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
                                && !detailListNoDuplicates.Contains(e.Almacen.Id.ToString() + e.Producto.Id.ToString())
                                //&& e.Almacen.Id == model.StoreId  // se Mejoro la consult GCHAVEZ 03/06/2023
                                                                               
                                )
                        .OrderBy(e => e.Producto).OrderByDescending(e => e.Almacen.Id) // se Mejoro la consult GCHAVEZ 09/06/2023
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

        //public async Task<ICollection<ProductIn>> ReportCompras(ComprasViewModel model)
        public async Task<ICollection<ProductIn>> ReportCompras(ComprasViewModel model)
        {
            List<ProductIn> result = new();

            if (model.StoreId != 0)
            {
                //if (model.StoreId == 0)
                //{
                //    model.StoreId = null;
                //}

                if (model.ContadoCompras && model.CreditCompras)
                {
                    result = await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Include(pi => pi.Almacen)
                    .Where(pi => pi.FechaIngreso >= model.Desde && pi.FechaIngreso <= model.Hasta && pi.Almacen.Id == model.StoreId)
                    .ToListAsync();
                    return result;
                }
                else if (model.ContadoCompras && model.CreditCompras == false)
                {
                    result = await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Include(p => p.Almacen)
                    .Where(
                        pi =>
                            pi.FechaIngreso >= model.Desde
                            && pi.FechaIngreso <= model.Hasta
                            && pi.Almacen.Id == model.StoreId
                            && pi.IsCanceled == true
                    )
                    .ToListAsync();
                    return result;
                }
                else
                {
                    result = await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Include(p => p.Almacen)
                    .Where(
                        pi =>
                            pi.FechaIngreso >= model.Desde
                            && pi.FechaIngreso <= model.Hasta
                            && pi.Almacen.Id == model.StoreId
                            && pi.IsCanceled == true
                    )
                    .ToListAsync();

                }
            }
            else {
                if (model.ContadoCompras && model.CreditCompras)
                {
                    result = await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Include(pi => pi.Almacen)
                    .Where(pi => pi.FechaIngreso >= model.Desde && pi.FechaIngreso <= model.Hasta)
                    .ToListAsync();
                    return result;
                }
                else if (model.ContadoCompras && model.CreditCompras == false)
                {
                    result = await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Include(p => p.Almacen)
                    .Where(
                        pi =>
                            pi.FechaIngreso >= model.Desde
                            && pi.FechaIngreso <= model.Hasta
                            && pi.IsCanceled == true
                    )
                    .ToListAsync();
                    return result;
                }
                else
                {
                    result = await _context.ProductIns
                    .Include(pi => pi.Provider)
                    .Include(p => p.Almacen)
                    .Where(
                        pi =>
                            pi.FechaIngreso >= model.Desde
                            && pi.FechaIngreso <= model.Hasta
                            && pi.IsCanceled == true
                    )
                    .ToListAsync();

                    }
                }
            return result;
               
        }

        public async Task<ICollection<TrasladoResponse>> ReportproductMovments(TrasladoInventarioViewModel model)
        {
            List<TrasladoResponse> responseList = new();



            if (model.StoreId != 0)
            {
                var result = await _context.ProductMovments
                    .Include(pm => pm.MovmentDetails.Where(pmd => pmd.AlmacenDestinoId == model.StoreId))
                    .ThenInclude(pmd => pmd.Producto)
                    .Include(pm => pm.User)
                    .Where(pd => pd.Fecha >= model.Desde && pd.Fecha <= model.Hasta)
                    .ToListAsync();

                foreach (var movment in result)
                {
                    if (movment.MovmentDetails.Count > 0)
                    {
                        decimal sumCC = 0;
                        decimal sumVM = 0;
                        decimal sumVD = 0;
                        List<string> almacenesList = new List<string>();
                        foreach (var detalle in movment.MovmentDetails)
                        {
                            var existencia = await _context.Existences.FirstOrDefaultAsync(
                                e =>
                                    e.Producto.Id == detalle.Producto.Id
                                    && e.Almacen.Id == detalle.AlmacenDestinoId
                            );
                            sumCC += existencia.PrecioCompra;
                            sumVD += existencia.PrecioVentaDetalle;
                            sumVM += existencia.PrecioVentaMayor;
                            almacenesList.Add(existencia.Almacen.Name);
                        }
                        TrasladoResponse response =
                            new()
                            {
                                Id = movment.Id,
                                Usuario = movment.User.FullName,
                                Concepto = movment.Concepto,
                                ProductCount = movment.MovmentDetails.Count,
                                Almacen = string.Join(", ", almacenesList.Distinct()),
                                SumCostoCompra = sumCC,
                                SumVentaDetalle = sumVD,
                                SumVentaMayor = sumVM,
                                Fecha = movment.Fecha
                            };
                        responseList.Add(response);
                    }
                }
                return responseList;
            }
            else
            {
                var result = await _context.ProductMovments
                    .Include(pm => pm.MovmentDetails)
                    .ThenInclude(pmd => pmd.Producto)
                    .Include(pm => pm.User)
                    .Where(pd => pd.Fecha >= model.Desde && pd.Fecha <= model.Hasta)
                    .ToListAsync();
                foreach (var movment in result)
                {
                    if (movment.MovmentDetails.Count > 0)
                    {
                        decimal sumCC = 0;
                        decimal sumVM = 0;
                        decimal sumVD = 0;
                        List<string> almacenesList = new List<string>();
                        foreach (var detalle in movment.MovmentDetails)
                        {
                            var existencia = await _context.Existences.FirstOrDefaultAsync(
                                e =>
                                    e.Producto.Id == detalle.Producto.Id
                                    && e.Almacen.Id == detalle.AlmacenDestinoId
                            );
                            sumCC += existencia.PrecioCompra;
                            sumVD += existencia.PrecioVentaDetalle;
                            sumVM += existencia.PrecioVentaMayor;
                            almacenesList.Add(existencia.Almacen.Name);

                        }
                        TrasladoResponse response =
                            new()
                            {
                                Id = movment.Id,
                                Usuario = movment.User.FullName,
                                Concepto = movment.Concepto,
                                ProductCount = movment.MovmentDetails.Count,
                                Almacen = string.Join(", ", almacenesList.Distinct()),
                                SumCostoCompra = sumCC,
                                SumVentaDetalle = sumVD,
                                SumVentaMayor = sumVM,
                                Fecha = movment.Fecha
                            };
                        responseList.Add(response);
                    }
                }
                return responseList;
            }
        }

        public async Task<IEnumerable<ProductosInventario>> GetProductosInventarioAsync(int? productID, int? storeID, int? tipoNegocioID, int? familiaID, bool? showststore, bool? OmitirStock)
        {
            try
            {
                var result = await uspProductsList(productID, storeID, tipoNegocioID, familiaID, showststore, OmitirStock);

                return (IEnumerable<ProductosInventario>)result;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones según tus necesidades
                throw ex;
            }
        }


        public async Task<List<ProductosInventario>> uspProductsList(int? productID, int? storeID, int? tipoNegocioID, int? familiaID, bool? showststore, bool? OmitirStock)

        {
            List<Store.Entities.ProductosInventario> result = new List<Store.Entities.ProductosInventario>();

            try
            {
                var sqlCommand = $@"[dbo].[GetProductosInventario] {productID},{storeID},{tipoNegocioID},{familiaID},{showststore},{OmitirStock}";
                result = await _context.Set<ProductosInventario>().FromSqlRaw(sqlCommand).ToListAsync();

            }
            catch (Exception ex)
            {

                return null;
            }
            return result;
        }

   
        public async Task<bool> GenerarHistoricoDocumentosPorCobrarAsync()
        {
            try
            {
                Console.WriteLine($"Iniciando generación de documentos: {DateTime.Now}");

                var mesAnterior = DateTime.Now.AddMonths(-1);
                var fechaInicio = new DateTime(mesAnterior.Year, mesAnterior.Month, 1);
                var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
                var historicalPeriod = $"{fechaInicio:yyyy-MM}";

                var registroencontrados = await _context.HistoricalReceivablesDocuments
                    .Where(h => h.Periodo == historicalPeriod)
                    .AnyAsync();

                if (registroencontrados)
                {
                    Console.WriteLine($"Ya existen registros para el período {historicalPeriod}");
                    return false;
                }

                var model = new CuentasXCobrarViewModel
                {
                    Desde = fechaInicio,
                    Hasta = fechaFin,
                    StoreId = 0,
                    ClientId = 0
                };

                var ventas = await ReportCuentasXCobrar(model);
                     var documentos = ventas.Select(v => new HistoricalReceivablesDocuments
                {
                    FechaVenta = v.FechaVenta,
                    FechaVencimiento = v.FechaVencimiento,
                    DiasAtraso = (int)(DateTime.Now - v.FechaVencimiento).TotalDays,
                    FacturaId = v.Id,
                    Almacen = v.Store?.Name?.Trim() ?? "Sin Almacén",
                    Cliente = v.Client?.NombreCliente?.Trim() ?? "Sin Cliente",
                    MontoVenta = v.MontoVenta,
                    TotalAbonado = v.MontoVenta - v.Saldo,
                    Saldo = v.Saldo,
                    FechaGeneracion = DateTime.Now,
                    Periodo = historicalPeriod,
                    IsContado = v.IsContado,
                    IsAnulado = v.IsAnulado
                }).ToList();

 
                foreach (var doc in documentos)
                {
                    if (doc.Almacen.Length > 100) 
                    {
                        doc.Almacen = doc.Almacen.Substring(0, 100);
                    }
                    if (doc.Cliente.Length > 200) 
                    {
                        doc.Cliente = doc.Cliente.Substring(0, 200);
                    }
                    doc.MontoVenta = Math.Round(doc.MontoVenta, 2);
                    doc.TotalAbonado = Math.Round(doc.TotalAbonado, 2);
                    doc.Saldo = Math.Round(doc.Saldo, 2);
                }

                try
                {
                    await _context.HistoricalReceivablesDocuments.AddRangeAsync(documentos);
                    await _context.SaveChangesAsync();

                    Console.WriteLine($"Documentos generados exitosamente: {DateTime.Now}");
                    Console.WriteLine($"Total documentos generados: {documentos.Count}");

                    return true;
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error al guardar en la base de datos: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Error interno: {ex.InnerException.Message}");
                    }

                    if (ex.Entries != null)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            Console.WriteLine($"Entidad con error: {entry.Entity.GetType().Name}");
                            Console.WriteLine($"Estado: {entry.State}");
                        }
                    }

                    throw; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return false;
            }
        }

        public async Task<ICollection<HistoricalReceivablesDocuments>> RHistoricalReceivablesDocuments(CuentasXCobrarViewModel model)
            {
            List<HistoricalReceivablesDocuments> result = new();

            try
            {
                string clientName = "";
                if(model.ClientId != 0)
                {
                    var client = await _context.Clients.FindAsync(model.ClientId);
                    clientName = client?.NombreCliente ?? "";

                }

                String nameStore = "";
                 if(model.StoreId != 0) 
                {
                    var store = await _context.Almacen.FindAsync(model.StoreId);
                    nameStore = store?.Name ?? "";
                }

                var query = _context.HistoricalReceivablesDocuments
                   .Where(h =>
                       h.FechaVenta.Date >= model.Desde.Date &&
                       h.FechaVenta.Date <= model.Hasta.Date &&
                       h.IsAnulado == false
                    );

                if (model.StoreId != 0) 
                {
                    query = query.Where(h => h.Cliente == clientName);
                }

                if (model.ClientId != 0)
                {
                    query = query.Where(h => h.Almacen == nameStore);

                }

                result = await query.ToListAsync();
                
                return result;

               }

            catch (Exception ex)
            {

                throw new Exception($"Error al Obtener el Documento Historico:{ex.Message}");
            }
        }
     }

  }


using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using NuGet.Packaging;
using Store.Data;
using Store.Entities;
using Store.Entities.CreateupdateConfig;
using Store.Entities.Logo;
using Store.Migrations;
using Store.Models.Responses;
using Store.Models.ViewModels;
using System;
using System.Data;

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
            DateTime hoy = DateTime.Now;

            return await _context.Sales
                     .Include(s => s.Client)
                     .Include(s => s.FacturedBy)
                     .Include(s => s.SaleDetails)
                     .ThenInclude(sd => sd.Store)
                     .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                     .ThenInclude(sd => sd.Product)
                     .Where(s => s.IsAnulado == false && s.Store.Id == idStore && s.IsContado)
                     .Select(
                    x =>
                        new Sales()
                        {
                            Id = x.Id,
                            IsEventual = x.IsEventual,
                            NombreCliente = x.NombreCliente,
                            Client = x.Client == null ? new Client
                            {
                                Id = 0,
                                NombreCliente = "CLIENTE EVENTUAL"
                            } : new Client
                            {
                                Id = x.Client.Id,
                                NombreCliente = x.Client.NombreCliente ?? "CLIENTE EVENTUAL"
                            },
                            ProductsCount = x.ProductsCount,
                            MontoVenta = x.MontoVenta,
                            IsDescuento = x.IsDescuento,
                            DescuentoXPercent = x.DescuentoXPercent,
                            DescuentoXMonto = x.DescuentoXMonto,
                            MontoVentaAntesDescuento = x.MontoVentaAntesDescuento,
                            FechaVenta = x.FechaVenta,
                            FacturedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            SaleDetails = x.SaleDetails
                                .Select(
                                    s =>
                                        new SaleDetail()
                                        {
                                            Id = s.Id,
                                            Store = s.Store == null ? new Almacen
                                            {
                                                Id = 0,
                                                Name = "Sin Almac�n"
                                            } : new Almacen
                                            {
                                                Id = s.Store.Id,
                                                Name = s.Store.Name ?? "Sin Nombre"
                                            },
                                            Product = s.Product == null ? new Producto
                                            {
                                                Id = 0,
                                                Description = "Producto No Encontrado"
                                            } : new Producto
                                            {
                                                Id = s.Product.Id,
                                                Description = s.Product.Description ?? "Sin Descripci�n"
                                            },
                                            Cantidad = s.Cantidad,
                                            IsDescuento = s.IsDescuento,
                                            CostoCompra = s.CostoCompra,
                                            DescuentoXPercent = s.DescuentoXPercent,
                                            Descuento = s.Descuento,
                                            CodigoDescuento = s.CodigoDescuento,
                                            Ganancia = s.Ganancia,
                                            CostoUnitario = s.CostoUnitario,
                                            PVM = s.PVM,
                                            PVD = s.PVD,
                                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                                            CostoTotalDespuesDescuento = s.CostoTotalDespuesDescuento,
                                            CostoTotal = s.CostoTotal,
                                            IsAnulado = s.IsAnulado,
                                            IsPartialAnulation = s.IsPartialAnulation,
                                            CantidadAnulada = s.CantidadAnulada,
                                            AnulatedBy = s.AnulatedBy,
                                            FechaAnulacion = s.FechaAnulacion
                                        }
                                )
                                .ToList(),
                            IsContado = x.IsContado,
                            IsCanceled = x.IsCanceled,
                            Saldo = x.Saldo,
                            FechaVencimiento = x.FechaVencimiento,
                            IsAnulado = x.IsAnulado,
                            AnulatedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            FechaAnulacion = x.FechaAnulacion,
                            Store = new Almacen() { Id = x.Store.Id, Name = x.Store.Name },
                            CodigoDescuento = x.CodigoDescuento
                        }
                )
                .ToListAsync();
        }


        public async Task<ICollection<Proformas>> GetContadoSalesByProfAsync()
        {
            DateTime hoy = DateTime.Now;

            var proformas = await _context.Proformas
                .AsNoTracking()
                .Include(p => p.Client) 
                .Include(p => p.FacturedBy) 
                .Include(p => p.Store) 
                .Include(p => p.ProformasDetails) 
                    .ThenInclude(pd => pd.Product) 
                .Include(p => p.ProformasDetails)
                    .ThenInclude(pd => pd.Store) 
                .Where(p => !p.IsContado 
                            && !p.IsCanceled 
                            && !p.IsAnulado
                            && p.TipoPago == null 
                            && p.FechaVenta.Year == hoy.Year 
                            && p.FechaVencimiento > hoy) 
                .OrderByDescending(p => p.FechaVenta) 
                .Select(p => new Proformas
                {
                    Id = p.Id,
                    IsEventual = p.IsEventual,
                    NombreCliente = p.Client != null ?(
                          !string.IsNullOrEmpty(p.Client.NombreCliente)? p.Client.NombreCliente:(
                            !string.IsNullOrEmpty(p.NombreCliente)? p.NombreCliente: "CLIENTE EVENTUAL"
                          )
                        )
                        :(
                          !string.IsNullOrEmpty(p.NombreCliente)? p.NombreCliente: "CLIENTE EVENTUAL"
                        ),
                    //NombreCliente = p.Client != null ? !string.IsNullOrEmpty(p.Client.NombreCliente) ? p.Client.NombreCliente : "CLIENTE EVENTUAL" : "CLIENTE EVENTUAL", 
                    Client = p.Client != null
                        ? new Client
                        {
                            Id = p.Client.Id,
                            NombreCliente = !string.IsNullOrEmpty(p.Client.NombreCliente) ? p.Client.NombreCliente  : "CLIENTE EVENTUAL"
                        }
                        : null,
                    ProductsCount = p.ProductsCount,
                    MontoVenta = p.MontoVenta,
                    IsDescuento = p.IsDescuento,
                    DescuentoXPercent = p.DescuentoXPercent,
                    DescuentoXMonto = p.DescuentoXMonto,
                    MontoVentaAntesDescuento = p.MontoVentaAntesDescuento,
                    FechaVenta = p.FechaVenta,
                    FacturedBy = p.FacturedBy != null
                        ? new Entities.User()
                        {
                            FirstName = p.FacturedBy.FirstName,
                            LastName = p.FacturedBy.LastName
                        }
                        : null,
                    ProformasDetails = p.ProformasDetails
                        .Where(s => !s.IsAnulado) // Filtra detalles no anulados
                        .Select(s => new ProformasDetails
                        {
                            Id = s.Id,
                            Store = new Almacen()
                            {
                                Id = s.Store.Id,
                                Name = s.Store.Name
                            },
                            Product = s.Product,
                            Cantidad = s.Cantidad,
                            IsDescuento = s.IsDescuento,
                            CostoCompra = s.CostoCompra,
                            DescuentoXPercent = s.DescuentoXPercent,
                            Descuento = s.Descuento,
                            CodigoDescuento = s.CodigoDescuento,
                            Ganancia = s.Ganancia,
                            CostoUnitario = s.CostoUnitario,
                            PVM = s.PVM,
                            PVD = s.PVD,
                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                            CostoTotalDespuesDescuento = s.CostoTotalDespuesDescuento,
                            CostoTotal = s.CostoTotal,
                            IsAnulado = s.IsAnulado,
                            IsPartialAnulation = s.IsPartialAnulation,
                            CantidadAnulada = s.CantidadAnulada,
                            AnulatedBy = s.AnulatedBy,
                            FechaAnulacion = s.FechaAnulacion
                        }
                                )
                                .ToList(),
                    IsContado = p.IsContado,
                    IsCanceled = p.IsCanceled,
                    Saldo = p.Saldo,
                    FechaVencimiento = p.FechaVencimiento,
                    IsAnulado = p.IsAnulado,
                    FechaAnulacion = p.FechaAnulacion,
                    Store = p.Store != null
                        ? new Almacen
                        {
                            Id = p.Store.Id,
                            Name = p.Store.Name
                        }
                        : null,
                    CodigoDescuento = p.CodigoDescuento
                })
                .ToListAsync();

            return proformas;
        }


        public async Task<ICollection<Proformas>> GetContadoSalesByProfAsync(int storeId)
        {
            DateTime hoy = DateTime.Now;

            var proformas = await _context.Proformas
                .AsNoTracking()
                .Include(p => p.Client)
                .Include(p => p.FacturedBy)
                .Include(p => p.Store)
                .Include(p => p.ProformasDetails)
                    .ThenInclude(pd => pd.Product)
                .Include(p => p.ProformasDetails)
                    .ThenInclude(pd => pd.Store)
                .Where(p => !p.IsContado
                            && !p.IsCanceled
                            && !p.IsAnulado
                && p.TipoPago == null
                            && p.Store.Id == storeId
                            && p.FechaVenta.Year == hoy.Year
                            && p.FechaVencimiento > hoy)
                .OrderByDescending(p => p.FechaVenta)
                .Select(p => new Proformas
                {
                    Id = p.Id,
                    IsEventual = p.IsEventual,
                    NombreCliente = p.Client != null ? (
                          !string.IsNullOrEmpty(p.Client.NombreCliente) ? p.Client.NombreCliente : (
                            !string.IsNullOrEmpty(p.NombreCliente) ? p.NombreCliente : "CLIENTE EVENTUAL"
                          )
                        )
                        : (
                          !string.IsNullOrEmpty(p.NombreCliente) ? p.NombreCliente : "CLIENTE EVENTUAL"
                        ),
                    //NombreCliente = p.Client != null ? !string.IsNullOrEmpty(p.Client.NombreCliente) ? p.Client.NombreCliente : "CLIENTE EVENTUAL" : "CLIENTE EVENTUAL", 
                    Client = p.Client != null
                        ? new Client
                        {
                            Id = p.Client.Id,
                            NombreCliente = !string.IsNullOrEmpty(p.Client.NombreCliente) ? p.Client.NombreCliente : "CLIENTE EVENTUAL"
                        }
                        : null,
                    ProductsCount = p.ProductsCount,
                    MontoVenta = p.MontoVenta,
                    IsDescuento = p.IsDescuento,
                    DescuentoXPercent = p.DescuentoXPercent,
                    DescuentoXMonto = p.DescuentoXMonto,
                    MontoVentaAntesDescuento = p.MontoVentaAntesDescuento,
                    FechaVenta = p.FechaVenta,
                    FacturedBy = p.FacturedBy != null
                        ? new Entities.User()
                        {
                            FirstName = p.FacturedBy.FirstName,
                            LastName = p.FacturedBy.LastName
                        }
                        : null,
                    ProformasDetails = p.ProformasDetails
                        .Where(s => !s.IsAnulado) // Filtra detalles no anulados
                        .Select(s => new ProformasDetails
                        {
                            Id = s.Id,
                            Store = new Almacen()
                            {
                                Id = s.Store.Id,
                                Name = s.Store.Name
                            },
                            Product = s.Product,
                            Cantidad = s.Cantidad,
                            IsDescuento = s.IsDescuento,
                            CostoCompra = s.CostoCompra,
                            DescuentoXPercent = s.DescuentoXPercent,
                            Descuento = s.Descuento,
                            CodigoDescuento = s.CodigoDescuento,
                            Ganancia = s.Ganancia,
                            CostoUnitario = s.CostoUnitario,
                            PVM = s.PVM,
                            PVD = s.PVD,
                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                            CostoTotalDespuesDescuento = s.CostoTotalDespuesDescuento,
                            CostoTotal = s.CostoTotal,
                            IsAnulado = s.IsAnulado,
                            IsPartialAnulation = s.IsPartialAnulation,
                            CantidadAnulada = s.CantidadAnulada,
                            AnulatedBy = s.AnulatedBy,
                            FechaAnulacion = s.FechaAnulacion
                        }
                                )
                                .ToList(),
                    IsContado = p.IsContado,
                    IsCanceled = p.IsCanceled,
                    Saldo = p.Saldo,
                    FechaVencimiento = p.FechaVencimiento,
                    IsAnulado = p.IsAnulado,
                    FechaAnulacion = p.FechaAnulacion,
                    Store = p.Store != null
                        ? new Almacen
                        {
                            Id = p.Store.Id,
                            Name = p.Store.Name
                        }
                        : null,
                    CodigoDescuento = p.CodigoDescuento
                })
                .ToListAsync();

            return proformas;
        }


        public async Task<ICollection<Sales>> GetCreditoSalesByStoreAsync(int idStore)
        {

            DateTime hoy = DateTime.Now;
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado == false))
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado == false && s.FechaVenta.Year <= hoy.Year && s.Store.Id == idStore && s.IsContado == false)
                .Select(
                    x =>
                        new Sales()
                        {
                            Id = x.Id,
                            IsEventual = x.IsEventual,
                            NombreCliente = x.NombreCliente,
                            Client =
                                x.Client != null
                                    ? new Client()
                                    {
                                        Id = x.Client.Id,
                                        NombreCliente = x.Client.NombreCliente
                                    }
                                    : new Client() { },
                            ProductsCount = x.ProductsCount,
                            MontoVenta = x.MontoVenta,
                            IsDescuento = x.IsDescuento,
                            DescuentoXPercent = x.DescuentoXPercent,
                            DescuentoXMonto = x.DescuentoXMonto,
                            MontoVentaAntesDescuento = x.MontoVentaAntesDescuento,
                            FechaVenta = x.FechaVenta,
                            FacturedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            SaleDetails = x.SaleDetails
                                .Select(
                                    s =>
                                        new SaleDetail()
                                        {
                                            Id = s.Id,
                                            Store = new Almacen()
                                            {
                                                Id = s.Store.Id,
                                                Name = s.Store.Name
                                            },
                                            Product = s.Product,
                                            Cantidad = s.Cantidad,
                                            IsDescuento = s.IsDescuento,
                                            CostoCompra = s.CostoCompra,
                                            DescuentoXPercent = s.DescuentoXPercent,
                                            Descuento = s.Descuento,
                                            CodigoDescuento = s.CodigoDescuento,
                                            Ganancia = s.Ganancia,
                                            CostoUnitario = s.CostoUnitario,
                                            PVM = s.PVM,
                                            PVD = s.PVD,
                                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                                            CostoTotalDespuesDescuento =
                                                s.CostoTotalDespuesDescuento,
                                            CostoTotal = s.CostoTotal,
                                            IsAnulado = s.IsAnulado,
                                            IsPartialAnulation = s.IsPartialAnulation,
                                            CantidadAnulada = s.CantidadAnulada,
                                            AnulatedBy = s.AnulatedBy,
                                            FechaAnulacion = s.FechaAnulacion
                                        }
                                )
                                .ToList(),
                            IsContado = x.IsContado,
                            IsCanceled = x.IsCanceled,
                            Saldo = x.Saldo,
                            FechaVencimiento = x.FechaVencimiento,
                            IsAnulado = x.IsAnulado,
                            AnulatedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            FechaAnulacion = x.FechaAnulacion,
                            Store = new Almacen() { Id = x.Store.Id, Name = x.Store.Name },
                            CodigoDescuento = x.CodigoDescuento
                        }
                )
                .ToListAsync();
        }

        public async Task<ICollection<Sales>> GetAnulatedSalesByStoreAsync(int idStore)
        {
            DateTime hoy = DateTime.Now;
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails.Where(sd => sd.IsAnulado))
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsAnulado && s.FechaVenta.Year <= hoy.Year && s.Store.Id == idStore && s.IsSaleCancelled == false)
                .Select(
                    x =>
                        new Sales()
                        {
                            Id = x.Id,
                            IsEventual = x.IsEventual,
                            NombreCliente = x.NombreCliente,
                            Client =
                                x.Client != null
                                    ? new Client()
                                    {
                                        Id = x.Client.Id,
                                        NombreCliente = x.Client.NombreCliente
                                    }
                                    : new Client() { },
                            ProductsCount = x.ProductsCount,
                            MontoVenta = x.MontoVenta,
                            IsDescuento = x.IsDescuento,
                            DescuentoXPercent = x.DescuentoXPercent,
                            DescuentoXMonto = x.DescuentoXMonto,
                            MontoVentaAntesDescuento = x.MontoVentaAntesDescuento,
                            FechaVenta = x.FechaVenta,
                            FacturedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            SaleDetails = x.SaleDetails
                                .Select(
                                    s =>
                                        new SaleDetail()
                                        {
                                            Id = s.Id,
                                            Store = new Almacen()
                                            {
                                                Id = s.Store.Id,
                                                Name = s.Store.Name
                                            },
                                            Product = s.Product,
                                            Cantidad = s.Cantidad,
                                            IsDescuento = s.IsDescuento,
                                            CostoCompra = s.CostoCompra,
                                            DescuentoXPercent = s.DescuentoXPercent,
                                            Descuento = s.Descuento,
                                            CodigoDescuento = s.CodigoDescuento,
                                            Ganancia = s.Ganancia,
                                            CostoUnitario = s.CostoUnitario,
                                            PVM = s.PVM,
                                            PVD = s.PVD,
                                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                                            CostoTotalDespuesDescuento =
                                                s.CostoTotalDespuesDescuento,
                                            CostoTotal = s.CostoTotal,
                                            IsAnulado = s.IsAnulado,
                                            IsPartialAnulation = s.IsPartialAnulation,
                                            CantidadAnulada = s.CantidadAnulada,
                                            AnulatedBy = s.AnulatedBy,
                                            FechaAnulacion = s.FechaAnulacion
                                        }
                                )
                                .ToList(),
                            IsContado = x.IsContado,
                            IsCanceled = x.IsCanceled,
                            Saldo = x.Saldo,
                            FechaVencimiento = x.FechaVencimiento,
                            IsAnulado = x.IsAnulado,
                            AnulatedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            FechaAnulacion = x.FechaAnulacion,
                            Store = new Almacen() { Id = x.Store.Id, Name = x.Store.Name },
                            CodigoDescuento = x.CodigoDescuento
                        }
                )
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
                .Select(
                    x =>
                        new Sales()
                        {
                            Id = x.Id,
                            IsEventual = x.IsEventual,
                            NombreCliente = x.NombreCliente,
                            Client =
                                x.Client != null
                                    ? new Client()
                                    {
                                        Id = x.Client.Id,
                                        NombreCliente = x.Client.NombreCliente
                                    }
                                    : new Client() { },
                            ProductsCount = x.ProductsCount,
                            MontoVenta = x.MontoVenta,
                            IsDescuento = x.IsDescuento,
                            DescuentoXPercent = x.DescuentoXPercent,
                            DescuentoXMonto = x.DescuentoXMonto,
                            MontoVentaAntesDescuento = x.MontoVentaAntesDescuento,
                            FechaVenta = x.FechaVenta,
                            FacturedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            SaleDetails = x.SaleDetails
                                .Select(
                                    s =>
                                        new SaleDetail()
                                        {
                                            Id = s.Id,
                                            Store = new Almacen()
                                            {
                                                Id = s.Store.Id,
                                                Name = s.Store.Name
                                            },
                                            Product = s.Product,
                                            Cantidad = s.Cantidad,
                                            IsDescuento = s.IsDescuento,
                                            CostoCompra = s.CostoCompra,
                                            DescuentoXPercent = s.DescuentoXPercent,
                                            Descuento = s.Descuento,
                                            CodigoDescuento = s.CodigoDescuento,
                                            Ganancia = s.Ganancia,
                                            CostoUnitario = s.CostoUnitario,
                                            PVM = s.PVM,
                                            PVD = s.PVD,
                                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                                            CostoTotalDespuesDescuento =
                                                s.CostoTotalDespuesDescuento,
                                            CostoTotal = s.CostoTotal,
                                            IsAnulado = s.IsAnulado,
                                            IsPartialAnulation = s.IsPartialAnulation,
                                            CantidadAnulada = s.CantidadAnulada,
                                            AnulatedBy = s.AnulatedBy,
                                            FechaAnulacion = s.FechaAnulacion
                                        }
                                )
                                .ToList(),
                            IsContado = x.IsContado,
                            IsCanceled = x.IsCanceled,
                            Saldo = x.Saldo,
                            FechaVencimiento = x.FechaVencimiento,
                            IsAnulado = x.IsAnulado,
                            AnulatedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            FechaAnulacion = x.FechaAnulacion,
                            Store = new Almacen() { Id = x.Store.Id, Name = x.Store.Name },
                            CodigoDescuento = x.CodigoDescuento
                        }
                )
                .ToListAsync();
        }


        public async Task<Sales> AddSaleAsync(AddSaleViewModel model, Entities.User user)   
        {
            DateTime hoy = DateTime.Now;
            if (model.IsEventual && !model.IsContado)
            {
                throw new Exception("No se puede realizar venta a cr�dito a clientes eventuales");
            }
            Client cl = null;
            int diasCredito = 0;
            
            if(!model.IsEventual)
            {
                cl = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient);


                if (cl == null)
                {
                    throw new Exception("Cliente no Encontrado");
                }
                if(!model.IsContado && cl.Valor <= 0)
                {
                    throw new Exception("El Cliente {cl.NombreCliente} no tiene configurado un per�odo de cr�dito v�lido");
                }


                diasCredito = cl.Valor;
                cl.ContadorCompras += 1;
                if (!model.IsContado)
                {
                    cl.CreditoConsumido += model.MontoVenta;
                }
                _context.Entry(cl).State = EntityState.Modified;
            }

            TipoPago tp = await _context.TipoPagos.FirstOrDefaultAsync(
                t => t.Id == model.TipoPagoId
            );

            Almacen store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.Storeid);

            Sales sale =
                new()
                {
                    IsEventual = model.IsEventual,
                    NombreCliente = model.IsEventual
                        ? model.NombreCliente == ""
                            ? "CLIENTE EVENTUAL"
                            : model.NombreCliente
                        : "",
                    Client = cl,
                    ProductsCount = model.SaleDetails.Count,
                    MontoVenta = model.MontoVenta,
                    IsDescuento = model.IsDescuento,
                    DescuentoXPercent = model.DescuentoXPercent,
                    DescuentoXMonto = model.DescuentoXMonto,
                    FechaVenta = hoy,
                    FacturedBy = user,
                    IsContado = model.IsEventual ? true : model.IsContado,
                    IsCanceled = model.IsEventual ? true : model.IsContado, //Si es de contado, esta cancelado
                    Saldo = model.IsEventual ? 0 : (model.IsContado ? 0 : model.MontoVenta),
                    //FechaVencimiento = hoy.AddDays(15),
                    Store = store,
                    FechaVencimiento = (model.IsEventual || model.IsContado) ? hoy : hoy.AddDays(diasCredito),
                    CodigoDescuento = model.CodigoDescuento,
                    MontoVentaAntesDescuento = model.MontoVentaAntesDescuento,
                    TipoPago = model.IsContado ? tp : null,
                    Reference = model.Reference
                };

            if (model.IsContado)
            {
                var movList = await _context.CajaMovments
                    .Where(c => c.Store.Id == model.Storeid && c.CajaTipo.Id == 1)
                    .ToListAsync();

                var mov = movList.Where(m => m.Id == movList.Max(k => k.Id)).FirstOrDefault();
            }

            List<SaleDetail> detalles = new();
            List<Kardex> KardexMovments = new();

            foreach (var item in model.SaleDetails)
            {
                Producto prod = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );

                //Modificamos las existencias
                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == prod && e.Almacen == store
                );
                existence.Existencia -= item.Cantidad;
                existence.PrecioVentaDetalle = item.PVD;
                existence.PrecioVentaMayor = item.PVM;
                _context.Entry(existence).State = EntityState.Modified;

                //Se crea el objeto detalle venta
                SaleDetail saleDetail =
                    new()
                    {
                        Store = store,
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
                Kardex kar = await _context.Kardex
                    .Where(k => k.Product == prod && k.Almacen == item.Store)
                    .OrderByDescending(k => k.Id)
                    .FirstOrDefaultAsync();

                int saldo = kar == null ? 0 : kar.Saldo;

                Kardex kardex =
                    new()
                    {
                        Product = prod,
                        Fecha = hoy,
                        Concepto = model.IsContado ? "VENTA DE CONTADO" : "VENTA DE CREDITO",
                        Almacen = store,
                        Entradas = 0,
                        Salidas = item.Cantidad,
                        Saldo = saldo - item.Cantidad,
                        User = user
                    };
                KardexMovments.Add(kardex);
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
                        Store = store,
                        TipoPago = tp,
                        Reference = model.Reference
                    };
                _context.Abonos.Add(abono);
            }

            //Unificamos objetos y los mandamos a la DB
            sale.SaleDetails = detalles;
            sale.KardexMovments = KardexMovments;
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


      
         public async Task<Proformas> FinishSalesAsync(int Id, int tipoPagoId, Entities.User user)
        {
            try
            {
                var Proformas = await _context.Proformas
                    .Include(s => s.Client)
                    .Include(s => s.FacturedBy)
                    .Include(s => s.ProformasDetails)
                        .ThenInclude(sd => sd.Product)
                    .Include(s => s.ProformasDetails)
                        .ThenInclude(sd => sd.Store)
                    .Include(s => s.Store)
                    .FirstOrDefaultAsync(s => s.Id == Id);

                if (Proformas == null)
                {
                    throw new Exception($"Venta {Id} no encontrada");
                }

                var tipoPago = await _context.TipoPagos
                    .FirstOrDefaultAsync(t => t.Id == tipoPagoId);

                if (tipoPago == null)
                {
                    throw new Exception($"Tipo de pago {tipoPagoId} no encontrado");
                }

                // Actualizar venta
                Proformas.IsContado = true;
                Proformas.IsCanceled = true;
                Proformas.TipoPago = tipoPago;
                Proformas.FechaVenta = DateTime.Now;
                Proformas.FacturedBy = user;

                _context.Entry(Proformas).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Proformas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al finalizar la venta: {ex.Message}");
            }
        }




        public async Task<ICollection<Abono>> GetQuoteListAsync(int id)
        {
            return await _context.Abonos
                .Include(a => a.Sale)
                .Include(s => s.RealizedBy)
                .Where(a => a.Sale.Id == id)
                .ToListAsync();
        }

        public async Task<ICollection<Abono>> AddAbonoAsync(AddAbonoViewModel model, Entities.User user )
        {
            DateTime hoy = DateTime.Now;
            Abono abono = new();
            List<Abono> abonoList = new();
            TipoPago tp = await _context.TipoPagos.FirstOrDefaultAsync(
                t => t.Id == model.IdTipoPago
            );

            decimal sobra = model.Monto;
            var sales = await _context.Sales
                .Include(c => c.Client)
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
                            ),
                            TipoPago = tp,
                            Reference = model.Reference
                        };

                        item.Saldo -= sobra;
                        if (item.Client.SaldoVencido > 0)
                        {
                            item.Client.SaldoVencido -= abono.Monto;
                        }
                        item.Client.CreditoConsumido -= abono.Monto;
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
                            ),
                            TipoPago = tp,
                            Reference = model.Reference
                        };

                        item.Saldo = 0;
                        item.IsCanceled = true;
                        if (item.Client.SaldoVencido > 0)
                        {
                            item.Client.SaldoVencido -= abono.Monto;
                            item.Client.FacturasVencidas -= 1;
                        }
                        item.Client.CreditoConsumido -= abono.Monto;
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
                            ),
                            TipoPago = tp,
                            Reference = model.Reference
                        };

                        abonoList.Add(abono);

                        sobra -= item.Saldo;
                        item.Saldo = 0;
                        item.IsCanceled = true;
                        if (item.Client.SaldoVencido > 0)
                        {
                            item.Client.SaldoVencido -= abono.Monto;
                            item.Client.FacturasVencidas -= 1;
                        }
                        item.Client.CreditoConsumido -= abono.Monto;
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
            await _context.SaveChangesAsync();
            return abonoList;
        }


        public async Task<bool> UpdateSaleProductsAsync(UpdateSaleDetailsViewModel model, Entities.User user)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.ProformasDetails == null || !model.ProformasDetails.Any())
                throw new ArgumentException("La lista de detalles no puede estar vac�a");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                DateTime hoy = DateTime.Now;

                // Cargar la proforma existente con sus detalles
                var proforma = await _context.Proformas
                    .Include(s => s.ProformasDetails)
                        .ThenInclude(sd => sd.Product)
                    .Include(s => s.Store)
                    .FirstOrDefaultAsync(s => s.Id == model.Id);

                if (proforma == null)
                    throw new Exception($"Proforma con ID {model.Id} no encontrada");

                var productIds = model.ProformasDetails.Select(d => d.ProductId).Distinct().ToList();
                var productos = await _context.Productos
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => p);

                // 1. Eliminar registros que ya no est�n en el modelo
                var detallesAEliminar = proforma.ProformasDetails
                    .Where(pd => !model.ProformasDetails.Any(md => md.ProductId == pd.Product.Id))
                    .ToList();

                foreach (var detalle in detallesAEliminar)
                {
                    _context.Entry(detalle).State = EntityState.Deleted;
                }

                // 2. Actualizar o agregar nuevos registros
                foreach (var modelDetail in model.ProformasDetails)
                {
                    //var producto = await _context.Productos
                    //    .FirstOrDefaultAsync(p => p.Id == modelDetail.ProductId);

                    if (!productos.TryGetValue(modelDetail.ProductId, out var producto))
                        throw new Exception($"Producto con ID {modelDetail.ProductId} no encontrado");

                    // Buscar si existe el detalle
                    var detalleExistente = proforma.ProformasDetails
                        .FirstOrDefault(pd => pd.Product.Id == modelDetail.ProductId);

                    if (detalleExistente != null)
                    {
                        // Actualizar detalle existente
                        detalleExistente.Cantidad = modelDetail.Cantidad;
                        detalleExistente.CostoUnitario = modelDetail.CostoUnitario;
                        detalleExistente.IsDescuento = modelDetail.IsDescuento;
                        detalleExistente.DescuentoXPercent = modelDetail.DescuentoXPercent;
                        detalleExistente.Descuento = modelDetail.Descuento;
                        detalleExistente.CodigoDescuento = modelDetail.CodigoDescuento;
                        detalleExistente.PVM = modelDetail.PVM;
                        detalleExistente.PVD = modelDetail.PVD;
                        detalleExistente.CostoTotalAntesDescuento = modelDetail.CostoTotalAntesDescuento;
                        detalleExistente.CostoTotalDespuesDescuento = modelDetail.CostoTotalDespuesDescuento;
                        detalleExistente.CostoTotal = modelDetail.CostoTotal;

                        //_context.Entry(detalleExistente).State = EntityState.Modified;
                    }
                    else
                    {
                        // Crear nuevo detalle
                        var nuevoDetalle = new ProformasDetails
                        {
                            Store = proforma.Store,
                            Product = producto,
                            Cantidad = modelDetail.Cantidad,
                            CostoUnitario = modelDetail.CostoUnitario,
                            IsDescuento = modelDetail.IsDescuento,
                            DescuentoXPercent = modelDetail.DescuentoXPercent,
                            Descuento = modelDetail.Descuento,
                            CodigoDescuento = modelDetail.CodigoDescuento,
                            PVM = modelDetail.PVM,
                            PVD = modelDetail.PVD,
                            CostoTotalAntesDescuento = modelDetail.CostoTotalAntesDescuento,
                            CostoTotalDespuesDescuento = modelDetail.CostoTotalDespuesDescuento,
                            CostoTotal = modelDetail.CostoTotal
                        };

                        proforma.ProformasDetails.Add(nuevoDetalle);
                    }
                }

                // 3. Actualizar totales de la proforma
                proforma.ProductsCount = model.ProformasDetails.Count;
                proforma.MontoVenta = model.ProformasDetails.Sum(d => d.CostoTotal);
                proforma.MontoVentaAntesDescuento = model.ProformasDetails.Sum(d => d.CostoTotalAntesDescuento);
                proforma.IsDescuento = model.ProformasDetails.Any(d => d.IsDescuento);

                if (proforma.IsDescuento)
                {
                    proforma.DescuentoXMonto = model.ProformasDetails.Sum(d => d.Descuento);
                    var detallesConDescuento = model.ProformasDetails.Where(d => d.IsDescuento).ToList();
                    if (detallesConDescuento.Any())
                    {
                        proforma.DescuentoXPercent = detallesConDescuento.Average(d => d.DescuentoXPercent);
                    }
                }
                else
                {
                    proforma.DescuentoXMonto = 0;
                    proforma.DescuentoXPercent = 0;
                }

                proforma.FechaVenta = hoy;
                proforma.FechaVencimiento = hoy.AddDays(15);

                //_context.Entry(proforma).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
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
            List<Kardex> KardexMovments = new();

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
                        Concepto = "DEVOLUCION DE VENTA",
                        Almacen = item.Store,
                        Entradas = item.Cantidad,
                        Salidas = 0,
                        Saldo = saldo + item.Cantidad,
                        User = user
                    };
                KardexMovments.Add(kardex);
            }

            sale.IsAnulado = true;
            sale.FechaAnulacion = hoy;
            sale.AnulatedBy = user;

            SaleAnulation saleAnulation =
                new()
                {
                    VentaAfectada = sale,
                    MontoAnulado = sale.MontoVenta,
                    FechaAnulacion = hoy,
                    AnulatedBy = user,
                    SaleAnulationDetails = saleAnulationDetailList,
                    Store = sale.Store,
                    KardexMovments = KardexMovments
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

        public async Task<Sales> AnularSaleforIdAsync(int id, Entities.User user)
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

            if (sale == null)
            {
                return sale;
            }

            // Anulamos los abonos
            var abono = await _context.Abonos.Where(a => a.Sale == sale).ToListAsync();
            foreach (var item in abono)
            {
                item.IsAnulado = true;
                _context.Entry(item).State = EntityState.Modified;
            }

            // Procesamos los detalles de la venta
            List<Kardex> KardexMovments = new();

            foreach (var item in sale.SaleDetails)
            {
                item.IsAnulado = true;
                item.AnulatedBy = user;
                item.FechaAnulacion = hoy;

                // Actualizamos existencias
                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == item.Product && e.Almacen == item.Store
                );
                existence.Existencia += item.Cantidad;
                _context.Entry(existence).State = EntityState.Modified;

                // Creamos movimiento de Kardex
                Kardex kar = await _context.Kardex
                    .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                    .OrderByDescending(k => k.Id)
                    .FirstOrDefaultAsync();

                int saldo = kar == null ? 0 : kar.Saldo;

                Kardex kardex = new()
                {
                    Product = item.Product,
                    Fecha = hoy,
                    Concepto = "ANULACION DE VENTA",
                    Almacen = item.Store,
                    Entradas = item.Cantidad,
                    Salidas = 0,
                    Saldo = saldo + item.Cantidad,
                    User = user
                };
                KardexMovments.Add(kardex);
            }

            // Actualizamos la venta
            sale.IsAnulado = true;
            sale.FechaAnulacion = hoy;
            sale.AnulatedBy = user;
            sale.IsCanceled = true; 
            sale.IsSaleCancelled = true; 

            // Actualizamos datos del cliente si no es eventual
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

            // Agregamos los movimientos de Kardex
            _context.Kardex.AddRange(KardexMovments);
            _context.Entry(sale).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Creamos los asientos contables
            List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

            // Asiento d�bito siempre va a ventas
            CountAsientoContableDetails detalleDebito = new()
            {
                Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74), // Cuenta de ventas
                Debito = sale.MontoVenta,
                Credito = 0
            };
            countAsientoContableDetailsList.Add(detalleDebito);

            // Para el cr�dito, diferenciamos seg�n el tipo de venta
            if (sale.IsContado)
            {
                // Si es de contado, NO afectamos la caja porque el dinero no entr�
                CountAsientoContableDetails detalleCredito = new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74), // Misma cuenta de ventas
                    Debito = 0,
                    Credito = sale.MontoVenta
                };
                countAsientoContableDetailsList.Add(detalleCredito);
            }
            else
            {
                // Si es cr�dito, afectamos cuentas por cobrar
                CountAsientoContableDetails detalleCredito = new()
                {
                    Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72), // Cuentas por cobrar
                    Debito = 0,
                    Credito = sale.MontoVenta
                };
                countAsientoContableDetailsList.Add(detalleCredito);
            }

            CountAsientoContable asientosContable = new()
            {
                Fecha = sale.FechaVenta,
                Referencia = $"ANULACION DE FACTURA: {sale.Id}",
                LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
                FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(f => f.Id == 3),
                Store = sale.Store,
                User = user,
                CountAsientoContableDetails = countAsientoContableDetailsList
            };

            _context.CountAsientosContables.Add(asientosContable);
            await _context.SaveChangesAsync();

            return sale;
        }

        //public async Task<Sales> AnularSaleforIdAsync(int id, Entities.User user)
        //{
        //    DateTime hoy = DateTime.Now;

        //    Sales sale = await _context.Sales
        //        .Include(s => s.Client)
        //        .Include(s => s.SaleDetails)
        //        .ThenInclude(sd => sd.Store)
        //        .Include(s => s.SaleDetails)
        //        .ThenInclude(sd => sd.Product)
        //        .Include(s => s.Store)
        //        .FirstOrDefaultAsync(s => s.Id == id);

        //    if (sale == null)
        //    {
        //        return sale;
        //    }

        //    // Anulamos los abonos
        //    var abono = await _context.Abonos.Where(a => a.Sale == sale).ToListAsync();
        //    foreach (var item in abono)
        //    {
        //        item.IsAnulado = true;
        //        _context.Entry(item).State = EntityState.Modified;
        //    }

        //    // Procesamos los detalles de la venta
        //    List<SaleAnulationDetails> saleAnulationDetailList = new();
        //    List<Kardex> KardexMovments = new();

        //    foreach (var item in sale.SaleDetails)
        //    {
        //        item.IsAnulado = true;
        //        item.AnulatedBy = user;
        //        item.FechaAnulacion = hoy;

        //        SaleAnulationDetails saleAnulationDetail = new()
        //        {
        //            FechaAnulacion = hoy,
        //            CantidadAnulada = item.Cantidad,
        //            SaleDetailAfectado = item
        //        };
        //        saleAnulationDetailList.Add(saleAnulationDetail);

        //        // Actualizamos existencias
        //        Existence existence = await _context.Existences.FirstOrDefaultAsync(
        //            e => e.Producto == item.Product && e.Almacen == item.Store
        //        );
        //        existence.Existencia += item.Cantidad;
        //        _context.Entry(existence).State = EntityState.Modified;

        //        // Creamos movimiento de Kardex
        //        Kardex kar = await _context.Kardex
        //            .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
        //            .OrderByDescending(k => k.Id)
        //            .FirstOrDefaultAsync();

        //        int saldo = kar == null ? 0 : kar.Saldo;

        //        Kardex kardex = new()
        //        {
        //            Product = item.Product,
        //            Fecha = hoy,
        //            Concepto = "ANULACION DE VENTA SIN EFECTIVO",
        //            Almacen = item.Store,
        //            Entradas = item.Cantidad,
        //            Salidas = 0,
        //            Saldo = saldo + item.Cantidad,
        //            User = user
        //        };
        //        KardexMovments.Add(kardex);
        //    }

        //    // Actualizamos la venta
        //    sale.IsAnulado = true;
        //    sale.FechaAnulacion = hoy;
        //    sale.AnulatedBy = user;

        //    // Creamos el registro de anulaci�n
        //    SaleAnulation saleAnulation = new()
        //    {
        //        VentaAfectada = sale,
        //        MontoAnulado = sale.MontoVenta,
        //        FechaAnulacion = hoy,
        //        AnulatedBy = user,
        //        SaleAnulationDetails = saleAnulationDetailList,
        //        Store = sale.Store,
        //        KardexMovments = KardexMovments
        //    };

        //    _context.SaleAnulations.Add(saleAnulation);

        //    // Actualizamos datos del cliente si no es eventual
        //    if (!sale.IsEventual)
        //    {
        //        sale.Client.ContadorCompras -= 1;
        //        sale.Client.CreditoConsumido -= sale.MontoVenta;

        //        if (hoy.Date < sale.FechaVencimiento.Date)
        //        {
        //            sale.Client.SaldoVencido -= sale.MontoVenta;
        //            sale.Client.FacturasVencidas -= 1;
        //        }
        //    }

        //    _context.Entry(sale).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    // Creamos los asientos contables
        //    List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

        //    // Asiento d�bito siempre va a ventas
        //    CountAsientoContableDetails detalleDebito = new()
        //    {
        //        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74), // Cuenta de ventas
        //        Debito = sale.MontoVenta,
        //        Credito = 0
        //    };
        //    countAsientoContableDetailsList.Add(detalleDebito);

        //    // Para el cr�dito, diferenciamos seg�n el tipo de venta
        //    if (sale.IsContado)
        //    {
        //        // Si es de contado, NO afectamos la caja porque el dinero no entr�
        //        CountAsientoContableDetails detalleCredito = new()
        //        {
        //            // Usar una cuenta de anulaciones o la misma cuenta de ventas
        //            Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74), // Misma cuenta de ventas
        //            Debito = 0,
        //            Credito = sale.MontoVenta
        //        };
        //        countAsientoContableDetailsList.Add(detalleCredito);
        //    }
        //    else
        //    {
        //        // Si es cr�dito, afectamos cuentas por cobrar
        //        CountAsientoContableDetails detalleCredito = new()
        //        {
        //            Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72), // Cuentas por cobrar
        //            Debito = 0,
        //            Credito = sale.MontoVenta
        //        };
        //        countAsientoContableDetailsList.Add(detalleCredito);
        //    }

        //    CountAsientoContable asientosContable = new()
        //    {
        //        Fecha = sale.FechaVenta,
        //        Referencia = $"ANULACION DE FACTURA SIN EFECTIVO: {sale.Id}",
        //        LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
        //        FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(f => f.Id == 3),
        //        Store = sale.Store,
        //        User = user,
        //        CountAsientoContableDetails = countAsientoContableDetailsList
        //    };

        //    _context.CountAsientosContables.Add(asientosContable);
        //    await _context.SaveChangesAsync();

        //    return sale;
        //}

        public async Task<ICollection<Sales>> GetdevolutionSalesByStoreAsync(int idStore)
        {
            return await _context.Sales
                .Include(s => s.Client)
                .Include(s => s.FacturedBy)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.IsSaleCancelled == true && s.Store.Id == idStore)
                .Select(
                    x =>
                        new Sales()
                        {
                            Id = x.Id,
                            IsEventual = x.IsEventual,
                            NombreCliente = x.NombreCliente,
                            Client =
                                x.Client != null
                                    ? new Client()
                                    {
                                        Id = x.Client.Id,
                                        NombreCliente = x.Client.NombreCliente
                                    }
                                    : new Client() { },
                            ProductsCount = x.ProductsCount,
                            MontoVenta = x.MontoVenta,
                            IsDescuento = x.IsDescuento,
                            DescuentoXPercent = x.DescuentoXPercent,
                            DescuentoXMonto = x.DescuentoXMonto,
                            MontoVentaAntesDescuento = x.MontoVentaAntesDescuento,
                            FechaVenta = x.FechaVenta,
                            FacturedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            SaleDetails = x.SaleDetails
                                .Select(
                                    s =>
                                        new SaleDetail()
                                        {
                                            Id = s.Id,
                                            Store = new Almacen()
                                            {
                                                Id = s.Store.Id,
                                                Name = s.Store.Name
                                            },
                                            Product = s.Product,
                                            Cantidad = s.Cantidad,
                                            IsDescuento = s.IsDescuento,
                                            CostoCompra = s.CostoCompra,
                                            DescuentoXPercent = s.DescuentoXPercent,
                                            Descuento = s.Descuento,
                                            CodigoDescuento = s.CodigoDescuento,
                                            Ganancia = s.Ganancia,
                                            CostoUnitario = s.CostoUnitario,
                                            PVM = s.PVM,
                                            PVD = s.PVD,
                                            CostoTotalAntesDescuento = s.CostoTotalAntesDescuento,
                                            CostoTotalDespuesDescuento =
                                                s.CostoTotalDespuesDescuento,
                                            CostoTotal = s.CostoTotal,
                                            IsAnulado = s.IsAnulado,
                                            IsPartialAnulation = s.IsPartialAnulation,
                                            CantidadAnulada = s.CantidadAnulada,
                                            AnulatedBy = s.AnulatedBy,
                                            FechaAnulacion = s.FechaAnulacion
                                        }
                                )
                                .ToList(),
                            //IsContado = x.IsContado,
                            IsCanceled = x.IsCanceled,
                            Saldo = x.Saldo,
                            FechaVencimiento = x.FechaVencimiento,
                            IsAnulado = x.IsAnulado,
                            AnulatedBy = new Entities.User()
                            {
                                FirstName = x.FacturedBy.FirstName,
                                LastName = x.FacturedBy.LastName
                            },
                            FechaAnulacion = x.FechaAnulacion,
                            Store = new Almacen() { Id = x.Store.Id, Name = x.Store.Name },
                            CodigoDescuento = x.CodigoDescuento
                        }
                )
                .ToListAsync();
        }






        //public async Task<Sales> GetdevolutionSalesByStoreAsync(int id, Entities.User user, string motivoAnulacion)
        //{
        //    DateTime hoy = DateTime.Now;

        //    Sales sale = await _context.Sales
        //        .Include(s => s.Client)
        //        .Include(s => s.SaleDetails)
        //        .ThenInclude(sd => sd.Store)
        //        .Include(s => s.SaleDetails)
        //        .ThenInclude(sd => sd.Product)
        //        .Include(s => s.Store)
        //        .FirstOrDefaultAsync(s => s.Id == id);

        //    if (sale == null)
        //    {
        //        return sale;
        //    }

        //    // Validar que la venta no est� ya anulada o devuelta
        //    if (sale.IsAnulado)
        //    {
        //        throw new InvalidOperationException("Esta venta ya ha sido devuelta");
        //    }

        //    // Crear lista para registrar detalles de anulaci�n
        //    List<SaleVoidDetails> saleVoidDetailsList = new();

        //    foreach (var item in sale.SaleDetails)
        //    {
        //        SaleVoidDetails saleVoidDetail = new()
        //        {
        //            FechaAnulacion = hoy,
        //            CantidadAnulada = item.Cantidad,
        //            SaleDetailAfectado = item,
        //            MotivoAnulacion = motivoAnulacion
        //        };

        //        saleVoidDetailsList.Add(saleVoidDetail);

        //        // No modificamos existencias porque es anulaci�n, no devoluci�n
        //        item.IsVentaAnulada = true;
        //        item.VentaAnuladaPorId = user.Id;
        //        item.FechaVentaAnulacion = hoy;
        //    }

        //    // Registrar la anulaci�n
        //    SaleVoid saleVoid = new()
        //    {
        //        VentaAfectada = sale,
        //        MontoAnulado = sale.MontoVenta,
        //        FechaAnulacion = hoy,
        //        AnuladoPor = user,
        //        MotivoAnulacion = motivoAnulacion,
        //        SaleVoidDetails = saleVoidDetailsList,
        //        Store = sale.Store
        //    };

        //    _context.SaleVoids.Add(saleVoid);

        //    // Actualizar la venta
        //    sale.IsVentaAnulada = true;
        //    sale.FechaVentaAnulacion = hoy;
        //    sale.VentaAnuladaPorId = user.Id;
        //    sale.MotivoAnulacion = motivoAnulacion;

        //    // Si hay pagos o abonos, marcarlos como anulados
        //    var abonos = await _context.Abonos.Where(a => a.Sale == sale).ToListAsync();
        //    foreach (var abono in abonos)
        //    {
        //        abono.IsVentaAnulada = true;
        //        _context.Entry(abono).State = EntityState.Modified;
        //    }

        //    // Asientos contables para la anulaci�n
        //    List<CountAsientoContableDetails> countAsientoContableDetailsList = new();

        //    // Agregar asientos contables espec�ficos para anulaci�n
        //    CountAsientoContableDetails detalleDebito = new()
        //    {
        //        Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74), // Ajustar seg�n corresponda
        //        Debito = sale.MontoVenta,
        //        Credito = 0
        //    };
        //    countAsientoContableDetailsList.Add(detalleDebito);

        //    // Asiento contable seg�n tipo de venta
        //    if (sale.IsContado)
        //    {
        //        CountAsientoContableDetails detalleCredito = new()
        //        {
        //            Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 66),
        //            Debito = 0,
        //            Credito = sale.MontoVenta
        //        };
        //        countAsientoContableDetailsList.Add(detalleCredito);
        //    }
        //    else
        //    {
        //        CountAsientoContableDetails detalleCredito = new()
        //        {
        //            Cuenta = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 72),
        //            Debito = 0,
        //            Credito = sale.MontoVenta
        //        };
        //        countAsientoContableDetailsList.Add(detalleCredito);
        //    }

        //    CountAsientoContable asientosContable = new()
        //    {
        //        Fecha = sale.FechaVenta,
        //        Referencia = $"ANULACI�N DE VENTA: {sale.Id} - {motivoAnulacion}",
        //        LibroContable = await _context.CountLibros.FirstOrDefaultAsync(c => c.Id == 4),
        //        FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(f => f.Id == 3),
        //        Store = sale.Store,
        //        User = user,
        //        CountAsientoContableDetails = countAsientoContableDetailsList
        //    };

        //    _context.CountAsientosContables.Add(asientosContable);
        //    _context.Entry(sale).State = EntityState.Modified;

        //    await _context.SaveChangesAsync();
        //    return sale;
        //}

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
            List<Kardex> KardexMovments = new();

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
                    Kardex kar = await _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                        .OrderByDescending(k => k.Id)
                        .FirstOrDefaultAsync();

                    int saldo = kar == null ? 0 : kar.Saldo;

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
                            Saldo = saldo + item.Cantidad,
                            User = user
                        };
                    KardexMovments.Add(kardex);
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
                //es mayor que cero
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

                    Kardex kar = await _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id && k.Almacen == item.Store)
                        .OrderByDescending(k => k.Id)
                        .FirstOrDefaultAsync();

                    int saldo = kar == null ? 0 : kar.Saldo;

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
                            Saldo = saldo + item.Cantidad,
                            User = user
                        };
                    KardexMovments.Add(kardex);
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
                    sale.Client.SaldoVencido -= montoAnulado;
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
                    Store = sale.Store,
                    KardexMovments = KardexMovments
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

        public async Task<ICollection<GetSalesAndQuotesResponse>> GetSalesUncanceledByClientAsync( int idClient )
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

        public async Task<Abono> AddAbonoEspecificoAsync( AddAbonoEspecificoViewModel model,  Entities.User user)
        {
            DateTime hoy = DateTime.Now;

            var sale = await _context.Sales
                .Include(s => s.Store)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == model.IdSale);
            sale.Saldo -= model.Monto;

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
                    Store = sale.Store,
                    TipoPago = await _context.TipoPagos.FirstOrDefaultAsync(
                        t => t.Id == model.IdTipoPago
                    ),
                    Reference = model.Reference
                };

            if (sale.Client.SaldoVencido > 0)
            {
                sale.Client.SaldoVencido -= abono.Monto;
            }
            sale.Client.CreditoConsumido -= abono.Monto;

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
        public async Task<Proformas> AddProformasAsync(AddProformasViewModel model, Entities.User user)
        {
            var store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.Storeid);

            // Crear la proforma principal
            Proformas proformas = new()
            {
                IsEventual = model.IsEventual,
                NombreCliente = model.NombreCliente,
                Client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.IdClient),
                ProductsCount = model.ProformasDetails.Count,
                MontoVenta = model.MontoVenta,
                IsDescuento = model.IsDescuento,
                DescuentoXMonto = model.DescuentoXMonto,
                DescuentoXPercent = model.DescuentoXPercent,
                MontoVentaAntesDescuento = model.MontoVentaAntesDescuento,
                FechaVenta = DateTime.Now,
                FechaVencimiento = DateTime.Now.AddDays(15), // Agregar fecha de vencimiento
                FacturedBy = user,
                IsContado = model.IsContado,
                IsCanceled = false,
                IsAnulado = false,
                Store = store,
                CodigoDescuento = model.CodigoDescuento,
                TipoPago = null, // Se establece cuando se finaliza la proforma
                Reference = model.Reference
            };

            // Crear la lista de detalles
            List<ProformasDetails> proformaDetailList = new();

            foreach (var item in model.ProformasDetails)
            {
                ProformasDetails proformaDetails = new()
                {
                    Store = store,
                    Product = await _context.Productos.FirstOrDefaultAsync(p => p.Id == item.Product.Id),
                    Cantidad = item.Cantidad,
                    IsDescuento = item.IsDescuento,
                    DescuentoXPercent = item.DescuentoXPercent,
                    Descuento = item.Descuento,
                    CodigoDescuento = item.CodigoDescuento ?? "",
                    CostoUnitario = item.CostoUnitario,
                    PVD = item.PVD,
                    PVM = item.PVM,
                    CostoTotalAntesDescuento = item.CostoTotalAntesDescuento,
                    CostoTotalDespuesDescuento = item.CostoTotalDespuesDescuento,
                    CostoTotal = item.CostoTotal,
                    IsAnulado = false,
                    IsPartialAnulation = false,
                    CantidadAnulada = 0,
                    CostoCompra = item.CostoCompra,
                    Ganancia = item.Ganancia
                };

                proformaDetailList.Add(proformaDetails);
            }

            proformas.ProformasDetails = proformaDetailList;

            // Agregar a la base de datos
            _context.Proformas.Add(proformas);
            await _context.SaveChangesAsync();

            // Cargar la proforma con los detalles completos
            var proformaWithDetails = await _context.Proformas
                .Include(p => p.Client)
                .Include(p => p.FacturedBy)
                .Include(p => p.Store)
                .Include(p => p.ProformasDetails)
                    .ThenInclude(pd => pd.Product) // Incluye los productos relacionados
                .Include(p => p.ProformasDetails)
                    .ThenInclude(pd => pd.Store) // Incluye el almac�n relacionado
                .FirstOrDefaultAsync(p => p.Id == proformas.Id);

            return proformaWithDetails;
        }



        public async Task<IEnumerable<Proformas>> ProformaListAsync(char Action)
        {
            try
            {
                var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };

                if (Action == '3') 
                {
                    var result = await _context.Set<Proformas>()
                    .FromSqlRaw("EXEC [dbo].[usp_Proforma] @Action = {0}, @Mensaje = {1} OUTPUT",
                                 Action,
                                 mensajeParam)
                    .ToListAsync();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en ProformaListAsync: {ex.Message}");
                throw;
            }

            return new List<Proformas>();
        }


        public async Task<Proformas> DeleteProformAsync(int Id, Entities.User user)

        {
            DateTime hoy = DateTime.Now;
        Proformas P = await _context.Proformas
          .Include(f => f.ProformasDetails)
           .FirstOrDefaultAsync(f => f.Id == Id);
            P.IsAnulado = true;
            P.FechaAnulacion = hoy;
            P.AnulatedBy = user;
            foreach (var item in P.ProformasDetails)
            {
                item.IsAnulado = true;
                item.FechaAnulacion = hoy;
                item.AnulatedBy = user;
            }
           _context.Entry(P).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return P;
        }




public async Task<Proformas> ProformaUpdateAsync(
      int id,
      int action,
      int storeId,
      string nombreCliente,
      string detalle,
      //int cantidad,
      //decimal precioUnitario,
      decimal total,
      decimal montoTotal,
      DateTime fechaEmision,
      DateTime fechaVencimiento,
      bool proformaRealizada,
      bool proformaVencida)
        {
            // Definir el par�metro de salida para el mensaje
            var mensajeParam = new SqlParameter("@Mensaje", SqlDbType.NVarChar, -1)
            {
                Direction = ParameterDirection.Output
            };

            // Crear y definir los dem�s par�metros
            var parameters = new[]
            {
        new SqlParameter("@Action", action),
        new SqlParameter("@Id", id),
        new SqlParameter("@StoreId", storeId),
        new SqlParameter("@NombreCliente", nombreCliente),
        new SqlParameter("@Detalle", detalle),
        //new SqlParameter("@Cantidad", cantidad),
        //new SqlParameter("@PrecioUnitario", precioUnitario),
        new SqlParameter("@Total", total),
        new SqlParameter("@MontoTotal", montoTotal),
        new SqlParameter("@FechaEmision", fechaEmision),
        new SqlParameter("@FechaVencimiento", fechaVencimiento),
        new SqlParameter("@ProformaRealizada", proformaRealizada),
        new SqlParameter("@ProformaVencida", proformaVencida),
        mensajeParam // El par�metro de salida
    };

            // Ejecutar el procedimiento almacenado para actualizar la proforma
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [dbo].[usp_Proforma] @Action, @Id, @StoreId, @NombreCliente, @Detalle, @Cantidad, @PrecioUnitario, @Total, @MontoTotal, @FechaEmision, @FechaVencimiento, @ProformaRealizada, @ProformaVencida, @Mensaje OUTPUT",
                parameters);

            // Manejar el mensaje de salida
            string mensaje = mensajeParam.Value.ToString();
            if (mensaje != "OK")
            {
                throw new Exception($"Error en la actualizaci�n de la proforma: {mensaje}"); // Proporciona un mensaje m�s informativo
            }

            // Retorna la proforma actualizada, asegurando que el Id sea v�lido
            var updatedProforma = await _context.Proformas.FindAsync(id); // O realiza otra consulta para obtener los detalles actualizados

            if (updatedProforma == null)
            {
                throw new Exception("La proforma actualizada no se encontr� en la base de datos."); // Proporciona un mensaje claro
            }

            return updatedProforma;
        }   

    }
}

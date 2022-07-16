using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.AsientoContHelper;
using Store.Models.ViewModels;
using StoreBackend.Models.ViewModels;

namespace Store.Helpers.EntradaProductos
{
    public class ProductsInHelper : IProductsInHelper
    {
        private readonly IAsientoContHelper _asientoContHelper;
        private readonly DataContext _context;

        public ProductsInHelper(IAsientoContHelper asientoContHelper, DataContext context)
        {
            _asientoContHelper = asientoContHelper;
            _context = context;
        }

        public async Task<ProductIn> AddProductInAsync(
            AddEntradaProductoViewModel model,
            Entities.User user
        )
        {
            DateTime fechaV = DateTime.Now;
            fechaV.AddDays(15);
            Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 1);
            Provider prov = await _context.Providers.FirstOrDefaultAsync(
                p => p.Id == model.ProviderId
            );
            ProductIn productIn =
                new()
                {
                    TipoEntrada = "CCOMPRA",
                    TipoPago = model.TipoPago,
                    NoFactura = model.NoFactura,
                    FechaIngreso = DateTime.Now,
                    FechaVencimiento = fechaV,
                    Provider = prov,
                    Almacen = alm,
                    CreatedBy = user.Id,
                    MontoFactura = model.MontoFactura,
                    IsCanceled = model.TipoPago != "Pago de Credito",
                };
            List<ProductInDetails> detalles = new();
            foreach (var item in model.ProductInDetails)
            {
                ProductInDetails pd = new();
                Producto prod = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );
                pd.Product = prod;
                pd.Cantidad = item.Cantidad;
                pd.CostoCompra = item.CostoCompra;
                pd.CostoUnitario = item.CostoUnitario;
                pd.Descuento = item.Descuento;
                pd.Impuesto = item.Impuesto;
                pd.PrecioVentaMayor = item.PrecioVentaMayor;
                pd.PrecioVentaDetalle = item.PrecioVentaDetalle;
                detalles.Add(pd);

                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == prod
                );
                if (existence == null)
                {
                    existence.Almacen = alm;
                    existence.Producto = prod;
                    existence.Existencia = pd.Cantidad;
                    existence.PrecioVentaMayor = item.PrecioVentaMayor;
                    existence.PrecioVentaDetalle = item.PrecioVentaDetalle;
                    existence.PrecioCompra = item.CostoUnitario;
                    _context.Add(existence);
                }
                else
                {
                    existence.Almacen = alm;
                    existence.Existencia += pd.Cantidad;
                    existence.PrecioVentaMayor = item.PrecioVentaMayor;
                    existence.PrecioVentaDetalle = item.PrecioVentaDetalle;
                    existence.PrecioCompra = item.CostoUnitario;
                    _context.Entry(existence).State = EntityState.Modified;
                }

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
                        Concepto =
                            model.TipoPago == "Pago de Credito"
                                ? "COMPRA DE CREDITO"
                                : "COMPRA DE CONTADO",
                        Almacen = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 1),
                        Entradas = item.Cantidad,
                        Salidas = 0,
                        Saldo = saldo + item.Cantidad,
                        User = user
                    };
                _context.Kardex.Add(kardex);
            }
            productIn.ProductInDetails = detalles;
            _context.ProductIns.Add(productIn);

            //Agregamos el registro contable
            List<AsientoContableDetailsViewModel> asientoContableDetails = new();
            AsientoContableDetailsViewModel detalleDebito =
                new()
                {
                    Count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 74),
                    Debito = productIn.MontoFactura,
                    Credito = 0,
                    Saldo = 0
                };
            asientoContableDetails.Add(detalleDebito);

            if (productIn.IsCanceled)
            {
                AsientoContableDetailsViewModel detalleCredito =
                    new()
                    {
                        Count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 70),
                        Debito = 0,
                        Credito = productIn.MontoFactura,
                        Saldo = 0
                    };
                asientoContableDetails.Add(detalleCredito);
            }
            else
            {
                AsientoContableDetailsViewModel detalleCredito =
                    new()
                    {
                        Count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 26),
                        Debito = 0,
                        Credito = productIn.MontoFactura,
                        Saldo = 0
                    };
                asientoContableDetails.Add(detalleCredito);
            }

            AddAsientoContableViewModel asientoCont =
                new()
                {
                    Referencia = $"COMPRA DE PRODUCTOS SEGUN FACTURA: {productIn.NoFactura}",
                    IdLibroContable = 4,
                    IdFuenteContable = 4,
                    AsientoContableDetails = asientoContableDetails,
                    Store = alm,
                };

            await _context.SaveChangesAsync();
            await _asientoContHelper.AddAsientoContable(asientoCont, user);
            return productIn;
        }

        public async Task<ProductIn> UpdateProductInAsync(
            UpdateEntradaProductoViewModel model,
            Entities.User user
        )
        {
            ProductIn productIn = await _context.ProductIns
                .Include(p => p.Provider)
                .Include(p => p.ProductInDetails)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefaultAsync(pI => pI.Id == model.Id);

            decimal diferenciaMonto = productIn.MontoFactura - model.MontoFactura;

            if (productIn == null)
            {
                return productIn;
            }

            DateTime fechaV = productIn.FechaIngreso.AddDays(15);
            Provider prov = await _context.Providers.FirstOrDefaultAsync(
                p => p.Id == model.ProviderId
            );
            productIn.TipoEntrada = model.TipoEntrada;
            productIn.TipoPago = model.TipoPago;
            productIn.FechaIngreso = model.FechaIngreso;
            productIn.FechaVencimiento = model.TipoPago == "Pago de Credito" ? fechaV : null;
            productIn.Provider = prov;
            productIn.EditDate = DateTime.Now;
            productIn.EditBy = user.Email;
            productIn.MontoFactura = model.MontoFactura;

            foreach (var item in model.ProductInDetails)
            {
                int diferencia = 0;
                ProductInDetails pd = await _context.ProductInDetails.FirstOrDefaultAsync(
                    p => p.Id == item.Id
                );

                Producto prod = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );

                //pd.Cantidad es lo que hay
                // item.Cantidad es lo que trae el modelo
                diferencia = item.Cantidad - pd.Cantidad;

                pd.CostoCompra = item.CostoCompra;
                pd.CostoUnitario = item.CostoUnitario;
                pd.Descuento = item.Descuento;
                pd.Impuesto = item.Impuesto;
                pd.PrecioVentaMayor = item.PrecioVentaMayor;
                pd.PrecioVentaDetalle = item.PrecioVentaDetalle;

                Existence existence = await _context.Existences.FirstOrDefaultAsync(
                    e => e.Producto == prod
                );

                if (diferencia == 0)
                {
                    pd.Cantidad = item.Cantidad;
                    _context.Entry(pd).State = EntityState.Modified;
                    existence.PrecioCompra = item.CostoUnitario;
                    existence.PrecioVentaDetalle = item.PrecioVentaDetalle;
                    existence.PrecioVentaMayor = item.PrecioVentaMayor;
                    _context.Entry(existence).State = EntityState.Modified;
                }
                //hay que restarle al sistema
                else if (diferencia < 0)
                {
                    int restar = pd.Cantidad - item.Cantidad;
                    pd.Cantidad = item.Cantidad;
                    _context.Entry(pd).State = EntityState.Modified;

                    existence.Existencia -= restar;
                    existence.PrecioCompra = item.CostoUnitario;
                    existence.PrecioVentaDetalle = item.PrecioVentaDetalle;
                    existence.PrecioVentaMayor = item.PrecioVentaMayor;
                    _context.Entry(prod).State = EntityState.Modified;

                    int totalEntradas = _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id)
                        .Sum(k => k.Entradas);

                    int totaSalidas = _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id)
                        .Sum(k => k.Salidas);

                    Kardex kardex =
                        new()
                        {
                            Product = prod,
                            Fecha = DateTime.Now,
                            Concepto = "MODIFICACION DE ORDEN",
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 1),
                            Entradas = 0,
                            Salidas = restar,
                            Saldo = totalEntradas - (totaSalidas + restar),
                            User = user
                        };
                    _context.Kardex.Add(kardex);
                }
                //Hay que sumarle al sistema
                else
                {
                    pd.Cantidad = item.Cantidad;
                    _context.Entry(pd).State = EntityState.Modified;

                    existence.Existencia += diferencia;
                    existence.PrecioCompra = item.CostoUnitario;
                    existence.PrecioVentaDetalle = item.PrecioVentaDetalle;
                    existence.PrecioVentaMayor = item.PrecioVentaMayor;
                    _context.Entry(prod).State = EntityState.Modified;

                    int totalEntradas = _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id)
                        .Sum(k => k.Entradas);

                    int totaSalidas = _context.Kardex
                        .Where(k => k.Product.Id == item.Product.Id)
                        .Sum(k => k.Salidas);

                    Kardex kardex =
                        new()
                        {
                            Product = prod,
                            Fecha = DateTime.Now,
                            Concepto = "MODIFICACION DE ORDEN",
                            Almacen = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 4),
                            Entradas = diferencia,
                            Salidas = 0,
                            Saldo = totalEntradas + diferencia - totaSalidas,
                            User = user
                        };
                    _context.Kardex.Add(kardex);
                }
                _context.Entry(prod).State = EntityState.Modified;
            }
            _context.Entry(productIn).State = EntityState.Modified;

            if (diferenciaMonto > 0)
            {
                List<AsientoContableDetailsViewModel> asientoContableDetails = new();
                AsientoContableDetailsViewModel aCDetailDeudora =
                    new()
                    {
                        Count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 25),
                        Debito = diferenciaMonto,
                        Credito = 0,
                        Saldo = 0
                    };
                asientoContableDetails.Add(aCDetailDeudora);

                AsientoContableDetailsViewModel aCDetailAcredora =
                    new()
                    {
                        Count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == 3),
                        Debito = 0,
                        Credito = diferenciaMonto,
                        Saldo = 0
                    };
                asientoContableDetails.Add(aCDetailAcredora);

                AddAsientoContableViewModel asientoCont =
                    new()
                    {
                        Referencia =
                            $"DEVOLUCION DE PRODUCTOS SEGUN FACTURA: {productIn.NoFactura}",
                        IdLibroContable = 4,
                        IdFuenteContable = 4,
                        AsientoContableDetails = asientoContableDetails,
                        Store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 4),
                    };

                await _asientoContHelper.AddAsientoContable(asientoCont, user);
            }
            await _context.SaveChangesAsync();
            return productIn;
        }
    }
}

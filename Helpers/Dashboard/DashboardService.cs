using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using System.Globalization;

namespace Store.Helpers.ClientService
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _context;

        public DashboardService(DataContext context)
        {
            _context = context;
        }

        public async Task<int> GetNewClientsByStoreAsync(int idStore)
        {
            var result = await _context.Clients
                .Where(
                    c =>
                        c.FechaRegistro.Year == DateTime.Now.Year
                        && c.FechaRegistro.Month == DateTime.Now.Month
                        && c.Store.Id == idStore
                )
                .ToListAsync();
            return result.Count;
        }

        public async Task<ICollection<GetSalesByDateResponse>> GetSalesByDateAsync(int idStore)
        {
            List<GetSalesByDateResponse> saleList = new();
            DateTime hoy = DateTime.Now;

            for (int i = 0; i < 7; i++)
            {
                int dia = (int)hoy.AddDays(-i).DayOfWeek;
                if (dia != 0)
                {
                    var sales = await _context.Sales
                        .Where(
                            s =>
                                s.IsAnulado == false
                                && s.FechaVenta.Date == hoy.AddDays(-i).Date
                                && s.Store.Id == idStore
                        )
                        .ToListAsync();

                    decimal contadoSales = 0;
                    decimal creditoSales = 0;
                    decimal recuperacion = 0;

                    foreach (var item in sales)
                    {
                        if (item.IsContado)
                        {
                            contadoSales += item.MontoVenta;
                        }
                        else
                        {
                            creditoSales += item.MontoVenta;
                        }
                    }

                    var abonos = await _context.Abonos
                        .Where(
                            a =>
                                a.IsAnulado == false
                                && a.FechaAbono.Date == hoy.AddDays(-i).Date
                                && a.Store.Id == idStore
                        )
                        .Include(a => a.Sale)
                        .ToListAsync();

                    foreach (var item in abonos)
                    {
                        if (item.Sale != null)
                        {
                            if (!item.Sale.IsContado)
                            {
                                recuperacion += item.Monto;
                            }
                        }
                    }

                    GetSalesByDateResponse data =
                        new()
                        {
                            Fecha = hoy.AddDays(-i),
                            Contado = contadoSales,
                            Credito = creditoSales,
                            Recuperacion = recuperacion
                        };
                    saleList.Add(data);
                }
            }

            return saleList;
        }

        public async Task<decimal> GetSalesMonthByStoreAsync(int idStore)
        {
            var sales = await _context.Abonos
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.FechaAbono.Month == DateTime.Now.Month
                        && s.Store.Id == idStore
                )
                .ToListAsync();
            return sales.Sum(s => s.Monto);
        }

        public async Task<ICollection<decimal>> GetSalesRecupMonthAsync(int idStore)
        {
            decimal contadoSales = 0;
            decimal creditoSales = 0;
            decimal recuperacion = 0;

            DateTime hoy = DateTime.Now;

            var sales = await _context.Sales
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.Store.Id == idStore
                        && s.FechaVenta.Year == hoy.Year
                        && s.FechaVenta.Month == hoy.Month
                )
                .ToListAsync();

            foreach (var item in sales)
            {
                if (item.IsContado)
                {
                    contadoSales += item.MontoVenta;
                }
                else
                {
                    creditoSales += item.MontoVenta;
                }
            }

            var abonos = await _context.Abonos
                .Where(
                    a =>
                        a.IsAnulado == false
                        && a.Store.Id == idStore
                        && a.FechaAbono.Year == hoy.Year
                        && a.FechaAbono.Month == hoy.Month
                )
                .Include(a => a.Sale)
                .ToListAsync();

            foreach (var item in abonos)
            {
                if (item.Sale != null)
                {
                    if (!item.Sale.IsContado)
                    {
                        recuperacion += item.Monto;
                    }
                }
            }

            List<decimal> result = new() { contadoSales, creditoSales, recuperacion };
            return result;
        }

        public async Task<decimal> GetSalesWeekByStoreAsync(int idStore)
        {
            decimal result = 0;
            DateTime hoy = DateTime.Now;
            int estaSemana = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(
                hoy,
                CalendarWeekRule.FirstDay,
                hoy.DayOfWeek
            );
            var sales = await _context.Abonos
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.FechaAbono.Month == hoy.Month
                        && s.Store.Id == idStore
                )
                .ToListAsync();
            foreach (var item in sales)
            {
                int saleEstaSemana = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(
                    item.FechaAbono,
                    CalendarWeekRule.FirstDay,
                    item.FechaAbono.DayOfWeek
                );
                if (estaSemana == saleEstaSemana)
                {
                    result += item.Monto;
                }
            }
            return result;
        }

        public async Task<ICollection<Client>> GetClientsByLocationAndStoreAsync(int idStore)
        {
            return await _context.Clients
                .Include(c => c.Community)
                .ThenInclude(com => com.Municipality)
                .Where(c => c.Store.Id == idStore)
                .ToListAsync();
        }

        public async Task<ICollection<int>> GetVisitedClientsByStoreAsync(int idStore)
        {
            List<int> clientList = new();
            int recurrente = 0;
            int nuevo = 0;
            int prospecto = 0;

            DateTime hoy = DateTime.Now;
            int estaSemana = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(
                hoy,
                CalendarWeekRule.FirstDay,
                hoy.DayOfWeek
            );

            var sales = await _context.Sales
                .Include(s => s.Client)
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.FechaVenta.Year == hoy.Year
                        && s.FechaVenta.Month == hoy.Month
                        && s.Store.Id == idStore
                        && s.Client != null
                )
                .ToListAsync();

            foreach (var item in sales)
            {
                int saleEstaSemana = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(
                    item.FechaVenta,
                    CalendarWeekRule.FirstDay,
                    item.FechaVenta.DayOfWeek
                );
                if (estaSemana == saleEstaSemana)
                {
                    Client cli = await _context.Clients.FirstOrDefaultAsync(
                        c => c.Id == item.Client.Id
                    );
                    if (cli.ContadorCompras == 1)
                    {
                        nuevo += 1;
                    }
                    else
                    {
                        recurrente += 1;
                    }
                }
            }

            var clients = await _context.Clients
                .Where(
                    c =>
                        c.Store.Id == idStore
                        && c.FechaRegistro.Year == hoy.Year
                        && c.FechaRegistro.Month == hoy.Month
                )
                .ToListAsync();
            foreach (var item in clients)
            {
                int clientEstaSemana = CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(
                    item.FechaRegistro,
                    CalendarWeekRule.FirstDay,
                    item.FechaRegistro.DayOfWeek
                );
                if (estaSemana == clientEstaSemana)
                {
                    if (item.ContadorCompras == 0)
                    {
                        prospecto += 1;
                    }
                }
            }
            clientList.Add(recurrente);
            clientList.Add(nuevo);
            clientList.Add(prospecto);
            return clientList;
        }

        public async Task<ICollection<Sales>> GetSalesByTNAndStoreAsync(int idStore)
        {
            var sales = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(s => s.Product.TipoNegocio)
                .Where(
                    s =>
                        s.Store.Id == idStore
                        && s.IsAnulado == false
                        && s.FechaVenta.Month == DateTime.Now.Month
                )
                .ToListAsync();

            return sales;

            // foreach (var item in sales)
            // {

            // }
        }
    }
}

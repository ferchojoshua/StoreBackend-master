using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;
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
                .Where(c => c.FechaRegistro.Month == DateTime.Now.Month && c.Store.Id == idStore)
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
                        .Include(s => s.SaleDetails.Where(sd => sd.Store.Id == idStore))
                        .ThenInclude(sd => sd.Store)
                        .Where(s => s.IsAnulado == false && s.FechaVenta.Day == hoy.AddDays(-i).Day)
                        .ToListAsync();

                    decimal contadoSales = 0;
                    decimal creditoSales = 0;
                    decimal recuperacion = 0;

                    foreach (var item in sales)
                    {
                        foreach (var detail in item.SaleDetails)
                        {
                            if (detail.Store.Id == idStore)
                            {
                                if (item.IsContado)
                                {
                                    contadoSales += item.MontoVenta;
                                }
                                else
                                {
                                    creditoSales += item.Saldo;
                                }
                            }
                        }
                    }

                    var salesOldest = await _context.Sales
                        .Where(s => s.IsAnulado == false && s.IsCanceled == false)
                        .ToListAsync();
                    foreach (var item in salesOldest)
                    {
                        recuperacion += _context.Abonos
                            .Where(
                                a =>
                                    a.IsAnulado == false
                                    && a.FechaAbono.Day == hoy.AddDays(-i).Day
                                    && a.Sale == item
                                    && a.Store.Id == idStore
                            )
                            .Sum(s => s.Monto);
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

            var sales = await _context.Sales
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Store)
                .Where(s => s.IsAnulado == false && s.FechaVenta.Month == DateTime.Now.Month)
                .ToListAsync();

            foreach (var item in sales)
            {
                foreach (var detail in item.SaleDetails)
                {
                    if (detail.Store.Id == idStore)
                    {
                        if (item.IsContado)
                        {
                            contadoSales += item.MontoVenta;
                        }
                        else
                        {
                            creditoSales += item.Saldo;
                        }
                    }
                }
            }

            var salesOldest = await _context.Sales
                .Where(
                    s =>
                        s.IsAnulado == false
                        && s.FechaVenta.Month <= DateTime.Now.Month
                        && s.IsCanceled == false
                )
                .ToListAsync();

            foreach (var item in salesOldest)
            {
                recuperacion += _context.Abonos
                    .Where(
                        a =>
                            a.IsAnulado == false
                            && a.FechaAbono.Month == DateTime.Now.Month
                            && a.Sale == item
                            && a.Store.Id == idStore
                    )
                    .Sum(s => s.Monto);
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
                        && s.FechaAbono.Month == DateTime.Now.Month
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
    }
}

using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Helpers.SalesHelper
{
    public class CashMovmentService : ICashMovmentService
    {
        private readonly DataContext _context;

        public CashMovmentService(DataContext context)
        {
            _context = context;
        }

        public async Task<CajaMovment> AddCashMovmentAsync(
            AddCashMovmentViewModel model,
            Entities.User user
        )
        {
            Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.AlmacenId);

            var cMList = await _context.CajaMovments
                .Where(c => c.Store == alm && c.CajaTipo.Id == 1)
                .ToListAsync();

            var lastCM = cMList.Where(c => c.Id == cMList.Max(cm => cm.Id)).FirstOrDefault();

            decimal entrada = 0;
            decimal salida = 0;
            decimal saldo = 0;
            if (model.IsEntrada)
            {
                entrada = model.Monto;
                saldo = lastCM.Saldo + model.Monto;
            }
            else
            {
                salida = model.Monto;
                saldo = lastCM.Saldo - model.Monto;
            }

            CajaMovment cM =
                new()
                {
                    Fecha = DateTime.Now,
                    Description = model.Description,
                    CajaTipo = await _context.CajaTipos.FirstOrDefaultAsync(c => c.Id == 1),
                    Entradas = entrada,
                    Salidas = salida,
                    Saldo = saldo,
                    RealizadoPor = user,
                    Store = alm
                };
            _context.CajaMovments.Add(cM);
            await _context.SaveChangesAsync();
            return cM;
        }

        public async Task<ICollection<CajaMovment>> GetCashMovmentByStoreAsync(int idStore)
        {
            return await _context.CajaMovments
                .Include(c => c.RealizadoPor)
                .Where(c => c.Store.Id == idStore && c.CajaTipo.Id == 1)
                .ToListAsync();
        }
    }
}

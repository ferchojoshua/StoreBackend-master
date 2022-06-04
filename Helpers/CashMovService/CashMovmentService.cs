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

        public async Task<ICollection<CajaMovment>> GetCashMovmentByStoreAsync(int idStore)
        {
            return await _context.CajaMovments
                .Include(c => c.RealizadoPor)
                .Where(c => c.Store.Id == idStore && c.CajaTipo.Id == 1)
                .ToListAsync();
        }
    }
}

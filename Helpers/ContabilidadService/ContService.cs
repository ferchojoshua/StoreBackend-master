using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using StoreBackend.Models.ViewModels;

namespace StoreBackend.Helpers.ContabilidadService
{
    public class ContService : IContService
    {
        private readonly DataContext _context;

        public ContService(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Count> AddCountAsync(AddCountViewModel model)
        {
            Count count =
                new()
                {
                    Descripcion = model.Description,
                    CountGroup = await _context.CountGroups.FirstOrDefaultAsync(
                        c => c.Id == model.IdCountGroup
                    ),
                    CountNumber = model.CountNumber,
                    IsActive = true
                };
            _context.Counts.Add(count);
            await _context.SaveChangesAsync();
            return count;
        }

        public async Task<Count> DeleteCountAsync(int idCuenta)
        {
            Count count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == idCuenta);
            if (count == null)
            {
                return count;
            }
            count.IsActive = false;
            _context.Entry(count).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return count;
        }

        public async Task<ICollection<CountFuentesContables>> GetCountFuentesContablesAsync()
        {
            return await _context.CountFuentesContables.ToListAsync();
        }

        public async Task<ICollection<CountGroup>> GetCountGroupsAsync()
        {
            return await _context.CountGroups.ToListAsync();
        }

        public async Task<ICollection<CountLibros>> GetCountLibrosAsync()
        {
            return await _context.CountLibros.ToListAsync();
        }

        public async Task<ICollection<Count>> GetCountListAsync()
        {
            return await _context.Counts
                .Where(c => c.IsActive == true)
                .Include(c => c.CountGroup)
                .ToListAsync();
        }

        public async Task<Count> UpdateCountAsync(UpdateCountViewModel model)
        {
            Count count = await _context.Counts.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (count == null)
            {
                return count;
            }
            count.CountNumber = model.CountNumber;
            count.Descripcion = model.Description;
            count.CountGroup = await _context.CountGroups.FirstOrDefaultAsync(
                c => c.Id == model.IdCountGroup
            );
            _context.Entry(count).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return count;
        }
    }
}

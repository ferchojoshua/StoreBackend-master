using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.ClientService;
using Store.Models.ViewModels;

namespace Store.Helpers.Locations
{
    public class LocationsHelper : ILocationsHelper
    {
        private readonly DataContext _context;

        public LocationsHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Community> AddCommunityAsync(AddCommunityViewModel model)
        {
            Community comm =
                new()
                {
                    Name = model.Name,
                    Municipality = await GetMunicipalityAsync(model.IdMunicipality)
                };
            _context.Communities.Add(comm);
            await _context.SaveChangesAsync();
            return comm;
        }

        public async Task<Department> GetDepartmentAsync(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Municipality> GetMunicipalityAsync(int id)
        {
            return await _context.Municipalities
                .Include(m => m.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<ICollection<Department>> GetDepartmentListAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<ICollection<Municipality>> GetMunicipalitiesByDeptoAsync(int id)
        {
            return await _context.Municipalities.Where(m => m.Department.Id == id).ToListAsync();
        }

        public async Task<ICollection<Community>> GetCommunitiesByMunAsync(int id)
        {
            return await _context.Communities.Where(c => c.Municipality.Id == id).ToListAsync();
        }

        public async Task<Community> UpdateCommunityAsync(UpdateCommunityViewModel model)
        {
            Community comm = await _context.Communities.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (comm == null)
            {
                return comm;
            }
            comm.Name = model.Name;
            _context.Entry(comm).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return comm;
        }

        public async Task<Community> DeleteCommunityAsync(int id)
        {
            Community comm = await _context.Communities.FirstOrDefaultAsync(c => c.Id == id);
            if (comm == null)
            {
                return comm;
            }
            _context.Communities.Remove(comm);
            await _context.SaveChangesAsync();
            return comm;
        }

        public async Task<Municipality> UpdateMunicipalityAsync(UpdateMunicipalityViewModel model)
        {
            var mun = await _context.Municipalities.FirstOrDefaultAsync(m => m.Id == model.Id);
            if (mun == null)
            {
                return mun;
            }
            mun.Abreviatura = model.Abreviatura;
            _context.Entry(mun).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return mun;
        }
    }
}

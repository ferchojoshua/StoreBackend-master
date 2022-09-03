using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Helpers.StoreService
{
    public class StoreHelper : IStoreHelper
    {
        private readonly DataContext _context;

        public StoreHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<Almacen> UpdateStoreAsync(Almacen model)
        {
            Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.Id);

            if (alm == null)
            {
                return alm;
            }
            alm.Name = model.Name;
            alm.Meta = model.Meta;
            _context.Entry(alm).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return alm;
        }
    }
}

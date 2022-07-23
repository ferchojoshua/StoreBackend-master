using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using StoreBackend.Models.ViewModels;

namespace Store.Helpers.AsientoContHelper
{
    public class AsientoContHelper : IAsientoContHelper
    {
        private readonly DataContext _context;

        public AsientoContHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<CountAsientoContable> AddAsientoContable(
            AddAsientoContableViewModel model,
            Entities.User user
        )
        {
            List<CountAsientoContableDetails> AsientoContDetails = new();
            foreach (var item in model.AsientoContableDetails)
            {
                CountAsientoContableDetails asientoContableDetail =
                    new()
                    {
                        Cuenta = item.Count,
                        Debito = item.Debito,
                        Credito = item.Credito,
                        Saldo = 0
                    };
                AsientoContDetails.Add(asientoContableDetail);
            }

            DateTime hoy = DateTime.Now;
            CountAsientoContable asientoContable =
                new()
                {
                    Fecha = hoy,
                    Referencia = model.Referencia,
                    LibroContable = await _context.CountLibros.FirstOrDefaultAsync(
                        c => c.Id == model.IdLibroContable
                    ),
                    CountAsientoContableDetails = AsientoContDetails,
                    FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(
                        f => f.Id == model.IdFuenteContable
                    ),
                    Store = model.Store,
                    User = user
                };
            _context.CountAsientosContables.Add(asientoContable);
            await _context.SaveChangesAsync();
            return asientoContable;
        }

        public async Task<ICollection<CountAsientoContable>> GetAsientoContableListAsync()
        {
            return await _context.CountAsientosContables
                .Include(c => c.LibroContable)
                .Include(c => c.FuenteContable)
                .Include(c => c.Store)
                .Include(c => c.User)
                .Include(c => c.CountAsientoContableDetails)
                .ThenInclude(cD => cD.Cuenta)
                .ToListAsync();
        }

        public async Task<CountAsientoContable> AddAtoContFromCtroller(
            AddAtoContViewModel model,
            Entities.User user
        )
        {
            List<CountAsientoContableDetails> AsientoContDetails = new();
            foreach (var item in model.AsientoContableDetails)
            {
                CountAsientoContableDetails asientoContableDetail =
                    new()
                    {
                        Cuenta = await _context.Counts.FirstOrDefaultAsync(
                            c => c.Id == item.CountId
                        ),
                        Debito = item.Debito,
                        Credito = item.Credito,
                        Saldo = 0
                    };
                AsientoContDetails.Add(asientoContableDetail);
            }

            CountAsientoContable asientoContable =
                new()
                {
                    Fecha = model.Fecha,
                    Referencia = model.Referencia,
                    LibroContable = await _context.CountLibros.FirstOrDefaultAsync(
                        c => c.Id == model.IdLibroContable
                    ),
                    CountAsientoContableDetails = AsientoContDetails,
                    FuenteContable = await _context.CountFuentesContables.FirstOrDefaultAsync(
                        f => f.Id == model.IdFuenteContable
                    ),
                    Store = await _context.Almacen.FirstOrDefaultAsync(s => s.Id == model.StoreId),
                    User = user
                };
            _context.CountAsientosContables.Add(asientoContable);
            await _context.SaveChangesAsync();
            return asientoContable;
        }
    }
}

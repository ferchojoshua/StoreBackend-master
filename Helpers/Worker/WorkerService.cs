using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Helpers.Worker
{
    public class WorkerService : IWorkerService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public WorkerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task CheckingAccessAsync()
        {
            // Create a new scope (since DbContext is scoped by default)
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var rolList = await GetRolesAsync();
        }

        public async Task<ICollection<Rol>> GetRolesAsync()
        {
            // Create a new scope (since DbContext is scoped by default)
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<DataContext>();
            return await _context.Rols.ToListAsync();
        }

        public async Task UpdateRolSessionAsync(Rol model)
        {
            // Create a new scope (since DbContext is scoped by default)
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<DataContext>();
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

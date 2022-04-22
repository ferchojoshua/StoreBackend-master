using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ClientService
{
    public class ClientsHelper : IClientsHelper
    {
        private readonly DataContext _context;

        public ClientsHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Client>> GetClientListAsync()
        {
            return await _context.Clients.Include(c => c.Community).ToListAsync();
        }

        public async Task<Client> GetClientAsync(int id)
        {
            return await _context.Clients
                .Include(c => c.Community)
                .ThenInclude(m => m.Municipality)
                .ThenInclude(d => d.Department)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> AddClientAsync(AddClientViewModel model, Entities.User user)
        {
            Community com = await _context.Communities.FirstOrDefaultAsync(
                c => c.Id == model.IdCommunity
            );
            Client cl =
                new()
                {
                    NombreCliente = model.NombreCliente,
                    Cedula = model.Cedula,
                    FechaRegistro = DateTime.Now,
                    Correo = model.Correo,
                    Telefono = model.Telefono,
                    Community = com,
                    Direccion = model.Direccion,
                    CreadoPor = user
                };
            _context.Clients.Add(cl);
            await _context.SaveChangesAsync();
            return cl;
        }
    }
}

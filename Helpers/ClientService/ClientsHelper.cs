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
            Client cl =
                new()
                {
                    NombreCliente = model.NombreCliente,
                    Cedula = model.Cedula,
                    FechaRegistro = DateTime.Now,
                    Correo = model.Correo,
                    Telefono = model.Telefono,
                    Community = await _context.Communities.FirstOrDefaultAsync(
                        c => c.Id == model.IdCommunity
                    ),
                    Direccion = model.Direccion,
                    CreadoPor = user,
                    Store = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == model.IdStore),
                    LimiteCredito = model.CreditLimit
                };
            _context.Clients.Add(cl);
            await _context.SaveChangesAsync();
            return cl;
        }

        public async Task<Client> UpdateClientAsync(UpdateClientViewModel model, Entities.User user)
        {
            Community com = await _context.Communities.FirstOrDefaultAsync(
                c => c.Id == model.IdCommunity
            );
            Client cl = await _context.Clients.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (cl == null)
            {
                return cl;
            }
            cl.NombreCliente = model.NombreCliente;
            cl.Cedula = model.Cedula;
            cl.Correo = model.Correo;
            cl.Telefono = model.Telefono;
            cl.Community = com;
            cl.Direccion = model.Direccion;
            cl.EditadoPor = user.UserName;
            cl.FechaEdicion = DateTime.Now;
            cl.LimiteCredito = model.CreditLimit;

            _context.Entry(cl).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cl;
        }

        public async Task<Client> DeleteClientAsync(int id)
        {
            Client cl = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (cl == null)
            {
                return cl;
            }
            _context.Clients.Remove(cl);
            await _context.SaveChangesAsync();
            return cl;
        }

        public async Task<ICollection<Client>> GetRoute(GetRouteClientViewModel model)
        {
            List<Client> clientsList = new();
            List<Client> clList = await _context.Clients
                .Include(c => c.Community)
                .ThenInclude(com => com.Municipality)
                .ToListAsync();
            foreach (var item in clList)
            {
                foreach (var municipality in model.MunicipalityList)
                {
                    if (item.Community.Municipality.Id == municipality.Id)
                    {
                        clientsList.Add(item);
                    }
                }
            }
            return clientsList;
        }
    }
}

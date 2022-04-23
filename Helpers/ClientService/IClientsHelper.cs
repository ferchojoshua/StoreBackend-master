using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ClientService
{
    public interface IClientsHelper
    {
        Task<ICollection<Client>> GetClientListAsync();
        Task<Client> GetClientAsync(int id);
        Task<Client> AddClientAsync(AddClientViewModel model, Entities.User user);
        Task<Client> UpdateClientAsync(UpdateClientViewModel model, Entities.User user);
        Task<Client> DeleteClientAsync(int id);
    }
}

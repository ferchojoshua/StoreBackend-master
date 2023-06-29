using Store.Entities;

namespace Store.Helpers.Worker
{
    public interface IWorkerService
    {
        Task CheckingAccessAsync();
        Task<ICollection<Rol>> GetRolesAsync();
        Task UpdateRolSessionAsync(Rol model);
    }
}

using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.ClientService
{
    public interface ILocationsHelper
    {
        Task<ICollection<Department>> GetDepartmentListAsync();
        Task<ICollection<Municipality>> GetMunicipalitiesByDeptoAsync(int id);
        Task<ICollection<Community>> GetCommunitiesByMunAsync(int id);
        Task<Department> GetDepartmentAsync(int id);
        Task<Municipality> GetMunicipalityAsync(int id);
        Task<Community> AddCommunityAsync(AddCommunityViewModel model);
    }
}

using Microsoft.AspNetCore.Identity;

namespace Store.Helpers.User
{

    public interface IUserHelper
    {

        Task<Entities.User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(Store.Entities.User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(Store.Entities.User user, string roleName);


    }
}

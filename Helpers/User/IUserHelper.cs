using Microsoft.AspNetCore.Identity;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.User
{
    public interface IUserHelper
    {
        Task<ICollection<Entities.User>> GetActiveUsersAsync();

        Task<ICollection<Entities.User>> GetInactiveUsersAsync();

        Task<ICollection<Entities.User>> GetAllUsersAsync();
        Task<Entities.User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(Entities.User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task LogoutUserAsync(Entities.User user);

        Task<IdentityResult> ChangePasswordAsync(
            Entities.User user,
            string oldPassword,
            string newPassword
        );

        Task<SignInResult> ValidatePasswordAsync(Entities.User user, string password);

        Task<IdentityResult> UpdateUserAsync(Entities.User user);

        Task<Entities.User> DeactivateUserAsync(string userName);

        Task<Entities.User> ResetPasswordAsync(string userName);

        //Roles
        Task<ICollection<Rol>> GetRolesAsync();

        Task<Rol> GetRoleAsync(string rolName);
        Task<Rol> CreateRoleAsync(AddRolViewModel model);
        Task<Rol> UpdateRoleAsync(Rol model);
        Task DeleteRolAsync(Rol rol);
        Task AddUserToRoleAsync(Entities.User user, int rolId);
    }
}

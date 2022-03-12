using Microsoft.AspNetCore.Identity;
using Store.Models.ViewModels;

namespace Store.Helpers.User
{
    public interface IUserHelper
    {
        Task<Entities.User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(Entities.User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(Entities.User user, string roleName);

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
    }
}

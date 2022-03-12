using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Models.ViewModels;

namespace Store.Helpers.User
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;

        private readonly UserManager<Entities.User> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<Entities.User> _signInManager;

        public UserHelper(
            DataContext context,
            UserManager<Entities.User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<Entities.User> signInManager
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(Entities.User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(Entities.User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        public async Task<Entities.User> GetUserAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false
            );
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task LogoutUserAsync(Entities.User user)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }

        //Valida Usuario, Ultimo parametro,
        //false, indica que el usuario no se bloquea despues de n intentos de login
        public async Task<SignInResult> ValidatePasswordAsync(Entities.User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            Entities.User user,
            string oldPassword,
            string newPassword
        )
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(Entities.User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}

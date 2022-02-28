using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;

namespace Store.Helpers.User
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<Store.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<Store.Entities.User> _signInManager;

        public UserHelper(DataContext context, UserManager<Store.Entities.User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<Store.Entities.User> signInManager)
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
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<Entities.User> GetUserAsync(string email)
        {
            return await _context.Users
                 .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}

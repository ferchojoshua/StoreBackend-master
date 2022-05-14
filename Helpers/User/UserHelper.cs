using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.ViewModels;

namespace Store.Helpers.User
{
    public class UserHelper : IUserHelper
    {
        private readonly DataContext _context;
        private readonly UserManager<Entities.User> _userManager;
        private readonly SignInManager<Entities.User> _signInManager;

        public UserHelper(
            DataContext context,
            UserManager<Entities.User> userManager,
            SignInManager<Entities.User> signInManager
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Metodos User
        public async Task<ICollection<Entities.User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive == true)
                .Include(u => u.Rol)
                .Include(u => u.StoreAccess)
                .ToListAsync();
        }

        public async Task<ICollection<Entities.User>> GetInactiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive == false)
                .Include(u => u.Rol)
                .Include(u => u.StoreAccess)
                .ToListAsync();
        }

        public async Task<ICollection<Entities.User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Rol)
                .Include(u => u.StoreAccess)
                .ToListAsync();
        }

        public async Task<IdentityResult> AddUserAsync(Entities.User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<Entities.User> GetUserAsync(string userName)
        {
            return await _context.Users
                .Where(u => u.IsActive == true)
                .Include(u => u.Rol)
                .Include(s => s.UserSession)
                .Include(u => u.StoreAccess)
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<Entities.User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .Where(u => u.IsActive == true)
                .Include(u => u.Rol)
                .ThenInclude(r => r.Permissions)
                .Include(u => u.UserSession)
                .Include(u => u.StoreAccess)
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user.UserSession == null)
            {
                await LogoutAsync();
                UserSession us =
                    new()
                    {
                        UserBrowser = "",
                        UserToken = "",
                        UserSO = ""
                    };
                user.UserSession = us;
                await UpdateUserAsync(user);
                await LogoutAsync();
                return user;
            }
            return user;
        }

        public async Task<bool> IsAutorized(Rol rol, string permiso)
        {
            bool result = true;
            Rol r = await _context.Rols
                .Where(r => r == rol)
                .Include(p => p.Permissions)
                .FirstOrDefaultAsync();
            Permission p = r.Permissions.FirstOrDefault(x => x.Description == permiso);
            if (!p.IsEnable)
            {
                return result = false;
            }
            return result;
        }

        public bool IsLogged(Entities.User user, string token)
        {
            bool result = false;

            return result;
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

        public async Task AddUserToRoleAsync(Entities.User user, int rolId)
        {
            Rol rol = await _context.Rols.Where(r => r.Id == rolId).FirstOrDefaultAsync();
            user.Rol = rol;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Entities.User> DeactivateUserAsync(string userName)
        {
            Entities.User user = await _context.Users
                .Where(u => u.UserName == userName)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return user;
            }
            user.IsActive = !user.IsActive;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Entities.User> ResetPasswordAsync(string id)
        {
            Entities.User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return user;
            }
            user.IsDefaultPass = true;
            await UpdateUserAsync(user);
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, "123456");
            await LogoutUserAsync(user);
            return user;
        }

        #endregion

        #region Metodos Roles
        public async Task<ICollection<Rol>> GetRolesAsync()
        {
            return await _context.Rols.Include(r => r.Permissions).ToListAsync();
        }

        public async Task<Rol> GetRoleAsync(string roleName)
        {
            return await _context.Rols
                .Include(r => r.Permissions)
                .Where(r => r.RoleName == roleName)
                .FirstOrDefaultAsync();
        }

        public async Task<Rol> CreateRoleAsync(AddRolViewModel model)
        {
            List<Permission> permisos = new();
            foreach (var item in model.Permissions)
            {
                Permission newPermiso =
                    new() { Description = item.Description, IsEnable = item.IsEnable };
                permisos.Add(newPermiso);
            }
            Rol newRol = new() { RoleName = model.RoleName, Permissions = permisos };
            _context.Add(newRol);
            await _context.SaveChangesAsync();
            return newRol;
        }

        public async Task<Rol> UpdateRoleAsync(Rol model)
        {
            Rol rol = await GetRoleAsync(model.RoleName);
            if (rol == null)
            {
                return rol;
            }
            rol.RoleName = model.RoleName;
            foreach (var item in model.Permissions)
            {
                Permission permiso = await _context.Permissions
                    .Where(p => p.Id == item.Id)
                    .FirstOrDefaultAsync();
                permiso.IsEnable = item.IsEnable;
                _context.Entry(permiso).State = EntityState.Modified;
            }
            _context.Entry(rol).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task DeleteRolAsync(Rol rol)
        {
            _context.RemoveRange(rol);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}

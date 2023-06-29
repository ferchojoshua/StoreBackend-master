using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public UsersController(IUserHelper userHelper, DataContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }

        [HttpGet]
        [Route("GetActiveUsers")]
        public async Task<IActionResult> GetActiveUsers()
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(user.Rol, "USER VER"))
            {
                return Unauthorized();
            }

            var users = await _userHelper.GetActiveUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetInactiveUsers")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(user.Rol, "USER VER"))
            {
                return Unauthorized();
            }

            var users = await _userHelper.GetInactiveUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(user.Rol, "USER VER"))
            {
                return Unauthorized();
            }

            var users = await _userHelper.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User usr = await _userHelper.GetUserByEmailAsync(email);
            if (usr.IsDefaultPass)
            {
                return Ok(usr);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (usr.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(usr);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(usr.Rol, "USER CREATE"))
            {
                return Unauthorized();
            }

            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                return BadRequest("Usuario ya existe");
            }
            try
            {
                List<Almacen> storelist = new();
                foreach (var item in model.Stores)
                {
                    Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == item.Id);
                    storelist.Add(alm);
                }

                user = new User
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    LastName = model.LastName,
                    SecondLastName = model.SecondLastName,
                    Email = $"{model.UserName}@automoto.com",
                    NormalizedEmail = $"{model.UserName}@automoto.com".ToUpper(),
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.UserName,
                    NormalizedUserName = model.UserName.ToUpper(),
                    Address = model.Address,
                    IsActive = true,
                    IsDefaultPass = true,
                    StoreAccess = storelist,
                    UserSession = new UserSession
                    {
                        UserBrowser = "",
                        UserToken = "",
                        UserSO = ""
                    }
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, model.RolId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User usr = await _userHelper.GetUserByEmailAsync(email);
            if (usr.IsDefaultPass)
            {
                return Ok(usr);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (usr.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(usr);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(usr.Rol, "USER UPDATE"))
            {
                return Unauthorized();
            }

            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Usuario no exite");
            }
            try
            {
                List<Almacen> storelist = new();
                foreach (var item in model.Stores)
                {
                    Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == item.Id);
                    storelist.Add(alm);
                }
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.LastName = model.LastName;
                user.SecondLastName = model.SecondLastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
                user.StoreAccess = storelist;
                await _userHelper.UpdateUserAsync(user);
                await _userHelper.AddUserToRoleAsync(user, model.RolId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeactivateUser/{userName}")]
        public async Task<IActionResult> DeactivateUser(string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User usr = await _userHelper.GetUserByEmailAsync(email);
            if (usr.IsDefaultPass)
            {
                return Ok(usr);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (usr.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(usr);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(usr.Rol, "USER DELETE"))
            {
                return Unauthorized();
            }

            User user = await _userHelper.DeactivateUserAsync(userName);
            if (user == null)
            {
                return BadRequest("Usuario no encontrado");
            }
            try
            {
                await _userHelper.UpdateUserAsync(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User usr = await _userHelper.GetUserByEmailAsync(email);
            if (usr.IsDefaultPass)
            {
                return Ok(usr);
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (usr.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(usr);
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(usr.Rol, "   USER UPDATE"))
            {
                return Unauthorized();
            }

            try
            {
                User user = await _userHelper.ResetPasswordAsync(id);
                if (user == null)
                {
                    return BadRequest("Usuario no encontrado");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

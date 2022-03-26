using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public UsersController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        [HttpGet]
        [Route("GetActiveUsers")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _userHelper.GetActiveUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetInactiveUsers")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var users = await _userHelper.GetInactiveUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
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

            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                return BadRequest("Usuario ya existe");
            }
            try
            {
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
                    UserSession = new UserSession { UserDevice = "", UserToken = "" }
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

            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Usuario no exite");
            }
            try
            {
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.LastName = model.LastName;
                user.SecondLastName = model.SecondLastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Address = model.Address;
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

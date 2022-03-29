using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Entities;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;

        public AccountController(IUserHelper userHelper, IConfiguration configuration)
        {
            _userHelper = userHelper;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        Claim[] claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };
                        SymmetricSecurityKey key =
                            new(Encoding.UTF8.GetBytes(_configuration["tokens:key"]));
                        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
                        JwtSecurityToken token =
                            new(
                                _configuration["tokens:Issuer"],
                                _configuration["tokens:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddMonths(3),
                                signingCredentials: credentials
                            );
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };

                        user.UserSession.UserToken = results.token;
                        user.UserSession.ExpirationDateToken = results.expiration;
                        user.UserSession.UserBrowser = model.UserBrowser;
                        user.UserSession.UserSO = model.UserSO;
                        await _userHelper.UpdateUserAsync(user);
                        return Created(String.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("CloseUserSession/{email}")]
        public async Task<IActionResult> CloseUserSession(string email)
        {
            User user = await _userHelper.GetUserAsync(email);
            try
            {
                await _userHelper.LogoutUserAsync(user);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
                User user = await _userHelper.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("01");
                }
                if (user.UserSession.UserToken != token)
                {
                    await _userHelper.LogoutAsync();
                    return Ok("eX01");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userHelper.LogoutAsync();
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ChangePass")]
        public async Task<IActionResult> ChangePass([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }
            try
            {
                await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                user.IsDefaultPass = false;
                await _userHelper.UpdateUserAsync(user);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("01");
            }
            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }
            try
            {
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.PhoneNumber = model.PhoneNumber;
                user.LastName = model.LastName;
                user.SecondLastName = model.SecondLastName;
                user.Address = model.Address;
                await _userHelper.UpdateUserAsync(user);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

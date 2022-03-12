using System;
using System.Reflection.Metadata;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Store.Data;
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

        private readonly DataContext _context;

        private readonly IConfiguration _configuration;

        public AccountController(
            IUserHelper userHelper,
            IConfiguration configuration,
            DataContext context
        )
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _context = context;
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
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await _userHelper.ValidatePasswordAsync(user, model.Password);
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
                                expires: DateTime.UtcNow.AddYears(5),
                                signingCredentials: credentials
                            );
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user
                        };
                        return Created(String.Empty, results);
                    }
                }
            }

            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return Ok(users);
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
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("01");
            }
            return Ok(user);
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
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("01");
            }
            try
            {
                await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                return Ok();
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

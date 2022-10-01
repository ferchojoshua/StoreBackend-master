using System.Security.Claims;
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
    public class RolesController : ControllerBase
    {
        private readonly IUserHelper _userHelper;

        public RolesController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
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
            if (!await _userHelper.IsAutorized(user.Rol, "ROLES VER"))
            {
                return Unauthorized();
            }

            var roles = await _userHelper.GetRolesAsync();
            return Ok(roles.OrderBy(r => r.RoleName));
        }

        [HttpPost]
        [Route("CreateRol")]
        public async Task<IActionResult> CreateRol([FromBody] AddRolViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
            if (!await _userHelper.IsAutorized(user.Rol, "ROLES CREATE"))
            {
                return Unauthorized();
            }

            Rol rol = await _userHelper.GetRoleAsync(model.RoleName);
            if (rol != null)
            {
                return NotFound("Este rol ya existe");
            }
            try
            {
                rol = await _userHelper.CreateRoleAsync(model);
                return Ok(rol);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateRol")]
        public async Task<IActionResult> UpdateRol([FromBody] Rol model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
            if (!await _userHelper.IsAutorized(user.Rol, "ROLES UPDATE"))
            {
                return Unauthorized();
            }

            try
            {
                Rol rol = await _userHelper.UpdateRoleAsync(model);
                if (rol == null)
                {
                    return NotFound("Este rol no existe");
                }
                return Ok(rol);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeleteRol/{rolName}")]
        // [Route("DeleteRol")]
        public async Task<IActionResult> DeleteRol(string rolName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

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
            if (!await _userHelper.IsAutorized(user.Rol, "ROLES DELETE"))
            {
                return Unauthorized();
            }
            Rol rol = await _userHelper.GetRoleAsync(rolName);
            if (rol == null)
            {
                return NotFound("Este rol no existe");
            }
            try
            {
                await _userHelper.DeleteRolAsync(rol);
                return Ok("Rol Eliminado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

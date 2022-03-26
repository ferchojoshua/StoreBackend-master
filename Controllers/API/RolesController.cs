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

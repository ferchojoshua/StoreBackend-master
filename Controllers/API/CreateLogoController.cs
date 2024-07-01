using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Entities.Logo;
using Store.Entities;
using Store.Helpers.CreateLogoHelper;
using Store.Helpers.CreateLogoHelperList;
using Store.Helpers.User;
using Store.Models.ViewModels.Logo;
using Store.Models.ViewModels;
using System.Security.Claims;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CreateLogoController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICreateLogoHelper _createLogoHelper;
        private readonly ICreateLogoHelperList _createLogoHelperList;
        private readonly DataContext _context;

        public CreateLogoController(IUserHelper userHelper, ICreateLogoHelper createLogoHelper, ICreateLogoHelperList createLogoHelperList, DataContext context)
        {
            _createLogoHelper = createLogoHelper;
            _userHelper = userHelper;
            _createLogoHelperList = createLogoHelperList;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<CreateLogo>> CreateLogo([FromForm] CreateLogoViewModel model)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }

            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            string token = HttpContext.Request.Headers["Authorization"].ToString();
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS CREATE"))
            {
                return Unauthorized();
            }

            try
            {
                if (model.Imagen == null || model.Imagen.Length == 0)
                {
                    return BadRequest("La imagen es requerida.");
                }

                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await model.Imagen.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                var result = await _createLogoHelper.CreateLogoAsync(
                    model.StoreId,
                    model.Direccion,
                    model.Ruc,
                    imageBytes,
                    model.Telefono,
                    model.TelefonoWhatsApp
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{storeId}")]
        public async Task<ActionResult<GetLogoViewModel>> GetLogoByStoreId(int storeId)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }

            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            string token = HttpContext.Request.Headers["Authorization"].ToString();
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var logoViewModel = await _createLogoHelperList.GetLogoByStoreIdAsync(storeId);

                if (logoViewModel == null)
                {
                    return NotFound($"No se encontró ningún logo para el storeId {storeId}");
                }

                return Ok(logoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el logo para el storeId {storeId}: {ex.Message}");
            }
        }
    }
}

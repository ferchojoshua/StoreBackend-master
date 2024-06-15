using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Entities;
using Store.Helpers.CreateLogoHelper;
using Store.Helpers.User;
using Store.Models.ViewModels.Logo;
using System.Security.Claims;

namespace Store.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateLogoController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICreateLogoHelper _createLogoHelper;
        private readonly DataContext _context;

        public CreateLogoController(IUserHelper userHelper, ICreateLogoHelper createLogoHelper, DataContext context)
        {
            _userHelper = userHelper;
            _createLogoHelper = createLogoHelper;
            _context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<CreateLogo>> CreateLogo([FromForm] CreateLogoViewModel model)
        {
            // Verifica si el usuario está autenticado y obtiene su email
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }

            // Obtiene el usuario por su email
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verifica si el usuario tiene una contraseña por defecto
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            // Verifica el token de la sesión del usuario
            string token = HttpContext.Request.Headers["Authorization"].ToString();
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            // Verifica si el usuario tiene permiso para crear logos
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS CREATE"))
            {
                return Unauthorized();
            }

            try
            {
                // Verifica si el archivo de imagen está presente
                if (model.Imagen == null || model.Imagen.Length == 0)
                {
                    return BadRequest("La imagen es requerida.");
                }

                // Convierte el archivo de imagen en un arreglo de bytes
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await model.Imagen.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                // Llama al helper para crear el logo
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
                // Retorna un mensaje de error si algo falla
                return BadRequest(ex.Message);
            }
        }
    }
}

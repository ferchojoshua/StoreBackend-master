using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Data;
using Store.Entities;
using Store.Entities.Ajustes;
using Store.Entities.Logo;
using Store.Helpers.CreateLogoHelper;
using Store.Helpers.User;
using Store.Models.ViewModels;
using Store.Models.ViewModels.Ajustes;
using Store.Models.ViewModels.Logo;
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
        
        
        public CreateLogoController(DataContext context, IUserHelper userHelper, ICreateLogoHelper createLogoHelper)
        {
            _createLogoHelper = createLogoHelper;
            _userHelper = userHelper;
     
        }

        [HttpPost("CreateLogo")]
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



        [HttpPost("UpdateLogo")]
        public async Task<ActionResult<UpdateLogo>> UpdateLogo([FromForm] UpdateLogoViewModel model)
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

                var result = await _createLogoHelper.UpdateLogoAsync(
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


        [HttpGet("GetLogoByStoreId/{storeId}")]
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
                var logoViewModel = await _createLogoHelper.GetLogoByStoreIdAsync(storeId);

                if (logoViewModel == null)
                {
                    return NotFound($"No se encontró ningún logo para el storeId {storeId}");
                }

                 if (logoViewModel.Imagen != null)
        {
            logoViewModel.ImagenBase64 = Convert.ToBase64String(logoViewModel.Imagen);  // Mostrar el logo seleccionado
        }


                return Ok(logoViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener el logo para el storeId {storeId}: {ex.Message}");
            }
        }


        [HttpPost("AjustesAdd")]
        public async Task<ActionResult<IEnumerable<Ajustes>>> AjustesAdd([FromBody] AjustesViewModel model)
        {
            // Verifica la autenticación del usuario
            string email = 
            User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }

            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verifica si el usuario tiene un token válido
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
                // Llama al método en el helper para procesar los ajustes
                var result = await _createLogoHelper.AjustesAsync(
                    model.Operacion,
                    model.Valor,
                    model.Catalogo,
                    model.Descripcion,
                    user.Email
                   
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            

        [HttpPost("AjustesUpdate")]
        public async Task<ActionResult<Ajustes>> AjustesUpdate([FromBody] AjustesViewModel model)

        {
            // Verifica la autenticación del usuario
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }

            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verifica si el usuario tiene un token válido
            string token = HttpContext.Request.Headers["Authorization"].ToString();
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS UPDATE"))
            {
                return Unauthorized();
            }

           


            try
            {



                // Llama al método en el helper para actualizar los ajustes
                var result = await _createLogoHelper.AjustesUpdateAsync(
                    model.Id,
                    model.Operacion, 
                    model.Valor,
                    model.Catalogo,
                    model.Descripcion,
                    (bool)model.Estado,
                    user.Email
                );  

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpPost("AjustesGetList")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Ajustes>>> AjustesGetList([FromBody] AjustesgetListViewModel model)

        {
            // Verifica la autenticación del usuario
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (email == null)
            {
                return Unauthorized();
            }

            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verifica si el usuario tiene un token válido
            string token = HttpContext.Request.Headers["Authorization"].ToString();
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }

            try
            {
                // Llama al método en el helper para obtener los catálogos
                var result = await _createLogoHelper.ObtenerCatalogosAsync(model.Operacion); 

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
    
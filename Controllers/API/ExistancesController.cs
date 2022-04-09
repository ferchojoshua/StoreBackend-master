#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.EntradaProductos;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ExistenceController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public ExistenceController(DataContext context, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _context = context;
        }

        // GET: api/ProductIns
        [HttpGet("{productId}")]
        public async Task<ActionResult<Existence>> GetExistenciesByProduct(int productId)
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "EXISTANCE VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }

            return await _context.Existences
                .Include(e => e.Almacen)
                .Include(e => e.Producto)
                .FirstOrDefaultAsync(e => e.Producto.Id == productId);
        }
    }
}

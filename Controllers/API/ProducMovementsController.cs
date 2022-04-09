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
    public class ProductMovementsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IProductsInHelper _productsInHelper;
        private readonly DataContext _context;

        public ProductMovementsController(
            DataContext context,
            IUserHelper userHelper,
            IProductsInHelper productsInHelper
        )
        {
            _userHelper = userHelper;
            _productsInHelper = productsInHelper;
            _context = context;
        }

          // GET: api/ProductIns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductMovments>>> GetProductIns()
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "PRODUCT TRANSLATE VER"))
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
            return await _context.ProductMovments
                .Include(p => p.Producto)
                .Include(p => p.User)
                .ToListAsync();
        }
    }
}
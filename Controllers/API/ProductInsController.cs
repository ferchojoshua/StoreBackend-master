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
    public class ProductInsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IProductsInHelper _productsInHelper;
        private readonly DataContext _context;

        public ProductInsController(
            DataContext context,
            IUserHelper userHelper,
            IProductsInHelper productsInHelper
        )
        {
            _userHelper = userHelper;
            _productsInHelper = productsInHelper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductIn>>> GetProductIns()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "ENTRADAPRODUCTOS VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                return await _context.ProductIns
                    .Include(p => p.Provider)
                    .OrderByDescending(p => p.Id)
                    .Include(p => p.ProductInDetails)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductIn>> GetProductIn(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "ENTRADAPRODUCTOS VER"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            var productIn = await _context.ProductIns
                .Include(p => p.Almacen)
                .Include(p => p.Provider)
                .Include(p => p.ProductInDetails)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefaultAsync(pI => pI.Id == id);

            if (productIn == null)
            {
                return NotFound();
            }

            return productIn;
        }

        [HttpPost]
        [Route("PutProductIn")]
        public async Task<IActionResult> PutProductIn(
            [FromBody] UpdateEntradaProductoViewModel model
        )
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
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
            if (!await _userHelper.IsAutorized(user.Rol, "ENTRADAPRODUCTOS UPDATE"))
            {
                return Unauthorized();
            }

            try
            {
                var productIn = await _productsInHelper.UpdateProductInAsync(model, user);
                if (productIn == null)
                {
                    return NotFound("Orden de compra no encontrada");
                }
                return Ok(productIn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductIn>> PostProductIn(
            [FromBody] AddEntradaProductoViewModel model
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "ENTRADAPRODUCTOS CREATE"))
            {
                return Unauthorized();
            }

            string token = HttpContext.Request.Headers["Authorization"];
            token = token["Bearer ".Length..].Trim();
            if (user.UserSession.UserToken != token)
            {
                await _userHelper.LogoutAsync(user);
                return Ok("eX01");
            }
            try
            {
                var productIn = await _productsInHelper.AddProductInAsync(model, user);
                return Ok(productIn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductIn(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
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
            if (!await _userHelper.IsAutorized(user.Rol, "ENTRADAPRODUCTOS DELETE"))
            {
                return Unauthorized();
            }
            var productIn = await _context.ProductIns.FindAsync(id);
            if (productIn == null)
            {
                return NotFound();
            }

            _context.ProductIns.Remove(productIn);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

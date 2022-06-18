using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.ProdMovements;
using Store.Helpers.ProductExistenceService;
using Store.Helpers.User;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ExistenceController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IProductMovementsHelper _pMHelper;
        private readonly IProdExistService _prodExistService;
        private readonly DataContext _context;

        public ExistenceController(
            DataContext context,
            IUserHelper userHelper,
            IProductMovementsHelper pMHelper,
            IProdExistService prodExistService
        )
        {
            _userHelper = userHelper;
            _pMHelper = pMHelper;
            _prodExistService = prodExistService;
            _context = context;
        }

        [HttpGet("GetExistencies")]
        public async Task<ActionResult<IEnumerable<ExistenciaResponse>>> GetExistencies()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
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

            try
            {
                var result = await _prodExistService.GetProductExistencesAsync();
                return Ok(result.OrderBy(p => p.Description));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetExistencesByProduct")]
        public async Task<ActionResult<Existence>> GetExistenciesByProduct(
            [FromBody] GetExistencesViewModel model
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

            Existence existence = await _context.Existences
                .Include(e => e.Almacen)
                .Include(e => e.Producto)
                .FirstOrDefaultAsync(
                    e => e.Producto.Id == model.IdProduct && e.Almacen.Id == model.IdAlmacen
                );
            if (existence == null)
            {
                existence = new()
                {
                    Almacen = await _context.Almacen.FirstOrDefaultAsync(
                        a => a.Id == model.IdAlmacen
                    ),
                    Producto = await _context.Productos.FirstOrDefaultAsync(
                        p => p.Id == model.IdProduct
                    ),
                    Existencia = 0,
                    PrecioVentaMayor = 0,
                    PrecioVentaDetalle = 0
                };
                _context.Existences.Add(existence);
                await _context.SaveChangesAsync();
            }
            return Ok(existence);
        }

        [HttpPost]
        [Route("GetExistencesByStore")]
        public async Task<ActionResult<ICollection<Existence>>> GetExistencesByStore(
            [FromBody] GetExistencesByStoreViewModel model
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

            var result = await _context.Existences
                .Include(e => e.Almacen)
                .Include(e => e.Producto)
                .ThenInclude(p => p.TipoNegocio)
                .Where(e => e.Almacen.Id == model.IdAlmacen)
                .ToListAsync();

            return Ok(result.OrderBy(e => e.Producto.Description));
        }

        [HttpPost]
        [Route("AddProductMover")]
        public async Task<ActionResult> AddProductMover(
            [FromBody] AddProductMovementViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "PRODUCT TRANSLATE CREATE"))
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

            try
            {
                var result = await _pMHelper.AddMoverProductAsync(model, user);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateProductExistence")]
        public async Task<ActionResult> UpdateProductExistence(
            [FromBody] UpdateExistencesByStoreViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "EXISTANCE UPDATE"))
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

            try
            {
                var result = await _pMHelper.UpdateProductExistencesAsync(model, user);
                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

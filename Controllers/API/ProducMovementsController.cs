using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities;
using Store.Helpers.ProdMovements;
using Store.Helpers.User;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMovementsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IProductMovementsHelper _productMovementsHelper;

        public ProductMovementsController(
            IUserHelper userHelper,
            IProductMovementsHelper productMovementsHelper
        )
        {
            _userHelper = userHelper;
            _productMovementsHelper = productMovementsHelper;
        }

        // GET: api/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductMovments>>> GetProductMovments()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
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
            try
            {
                var result = await _productMovementsHelper.GetProductMovmentsAsync();
                return Ok(result.OrderByDescending(r => r.Fecha));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            // var result = await _context.ProductMovments
            //     // .Include(p => p.MovmentDetails)
            //     .Include(p => p.User)
            //     .ToListAsync();

            // List<ProductMovementsResponse> pMList = new();
            // foreach (var item in result)
            // {
            //     ProductMovementsResponse pM =
            //         new()
            //         {
            //             Id = item.Id,
            //             // Producto = item.Producto,
            //             AlmacenProcedencia = await _context.Almacen.FirstOrDefaultAsync(
            //                 a => a.Id == item.AlmacenProcedenciaId
            //             ),
            //             AlmacenDestino = await _context.Almacen.FirstOrDefaultAsync(
            //                 a => a.Id == item.AlmacenDestinoId
            //             ),
            //             // Cantidad = item.Cantidad,
            //             // Concepto = item.Concepto,
            //             User = item.User,
            //             Fecha = item.Fecha
            //         };
            //     pMList.Add(pM);
            // }
            // return Ok(pMList.OrderByDescending(p => p.Fecha));
        }
    }
}

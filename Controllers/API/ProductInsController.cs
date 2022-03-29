#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
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
        private readonly DataContext _context;

        public ProductInsController(DataContext context, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _context = context;
        }

        // GET: api/ProductIns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductIn>>> GetProductIns()
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }
            return await _context.ProductIns
                .Include(p => p.Provider)
                .OrderByDescending(p => p.Id)
                .Include(p => p.ProductInDetails)
                .ToListAsync();
        }

        // GET: api/ProductIns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductIn>> GetProductIn(int id)
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
                await _userHelper.LogoutAsync();
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

        // PUT: api/ProductIns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductIn(int id, ProductIn productIn)
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
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(user.Rol, "ENTRADAPRODUCTOS UPDATE"))
            {
                return Unauthorized();
            }
            if (id != productIn.Id)
            {
                return BadRequest();
            }

            _context.Entry(productIn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductInExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductIns
        [HttpPost]
        public async Task<ActionResult<ProductIn>> PostProductIn(AddEntradaProductoViewModel model)
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
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
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }
            DateTime fechaV = DateTime.Now;
            fechaV.AddDays(15);
            Almacen alm = await _context.Almacen.FirstOrDefaultAsync(a => a.Id == 1);
            Provider prov = await _context.Providers.FirstOrDefaultAsync(
                p => p.Id == model.Provider.Id
            );
            ProductIn productIn = new();
            productIn.TipoEntrada = model.TipoEntrada;
            productIn.TipoPago = model.TipoPago;
            productIn.NoFactura = model.NoFactura;
            productIn.FechaIngreso = DateTime.Now;
            productIn.FechaVencimiento = model.TipoPago == "Pago de Credito" ? fechaV : null;
            productIn.Provider = prov;
            productIn.Almacen = alm;
            productIn.MontoFactura = model.MontoFactura;
            List<ProductInDetails> detalles = new();
            foreach (var item in model.ProductInDetails)
            {
                ProductInDetails pd = new();
                pd.Product = await _context.Productos.FirstOrDefaultAsync(
                    p => p.Id == item.Product.Id
                );
                pd.Cantidad = item.Cantidad;
                pd.CostoCompra = item.CostoCompra;
                pd.CostoUnitario = item.CostoUnitario;
                pd.Descuento = item.Descuento;
                pd.Impuesto = item.Impuesto;
                pd.PrecioVentaMayor = item.PrecioVentaMayor;
                pd.PrecioVentaDetalle = item.PrecioVentaDetalle;
                detalles.Add(pd);
            }
            productIn.ProductInDetails = detalles;
            _context.ProductIns.Add(productIn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductIn", new { id = productIn.Id }, productIn);
        }

        // DELETE: api/ProductIns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductIn(int id)
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
                await _userHelper.LogoutAsync();
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

        private bool ProductInExists(int id)
        {
            return _context.ProductIns.Any(e => e.Id == id);
        }
    }
}

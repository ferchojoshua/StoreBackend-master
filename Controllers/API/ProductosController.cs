﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Store.Data;
using Store.Entities;
using Store.Entities.ProductoRecal;
using Store.Helpers.ProductHelper;
using Store.Helpers.User;
using Store.Models.ViewModels;
using Store.Models.ViewModels.Logo;
using Store.Models.ViewModels.Masivo;
using System.Security.Claims;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IProductHelper _productHelper;
        private readonly DataContext _context;

        public ProductosController(
            DataContext context,
            IUserHelper userHelper,
            IProductHelper productHelper
        )
        {
            _userHelper = userHelper;
            _productHelper = productHelper;
            _context = context;
        }

        // GET: api/Productos Reutilizada por Recall
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS VER"))
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
            return await _context.Productos
                .Include(p => p.TipoNegocio)
                .Include(p => p.Familia)
                .Select(
                    x =>
                        new Producto()
                        {
                            Id = x.Id,
                            TipoNegocio = new TipoNegocio()
                            {
                                Id = x.TipoNegocio.Id,
                                Description = x.TipoNegocio.Description
                            },
                            Familia = x.Familia,
                            Description = x.Description,
                            BarCode = x.BarCode,
                            Marca = x.Marca,
                            Modelo = x.Modelo,
                            UM = x.UM
                        }
                )
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }

        [HttpGet("GetProductsRecalById/{id}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductsRecalById(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS VER"))
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
                var Producto = await _productHelper.GetProductsRecalByIdAsync(id);
                return Ok(Producto.OrderByDescending(s => s.Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // GET: api/Productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS VER"))
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
            Producto producto = await _context.Productos
                .Include(p => p.TipoNegocio)
                .Include(p => p.Familia)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductViewModel model)
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
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS UPDATE"))
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
                var prod = await _productHelper.UpdateProductAsync(model);
                return Ok(prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
         public async Task<ActionResult<Producto>> PostProducto([FromBody] ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Validación de usuario y permisos
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS CREATE"))
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
                var result = await _productHelper.AddProductAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Productos
        //[HttpPost]

        //public async Task<ActionResult<Producto>> PostProducto([FromBody] ProductViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    string email = User.Claims
        //        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
        //        .Value;
        //    User user = await _userHelper.GetUserByEmailAsync(email);
        //    if (user.IsDefaultPass)
        //    {
        //        return Ok(user);
        //    }
        //    if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS CREATE"))
        //    {
        //        return Unauthorized();
        //    }

        //    string token = HttpContext.Request.Headers["Authorization"];
        //    token = token["Bearer ".Length..].Trim();
        //    if (user.UserSession.UserToken != token)
        //    {
        //        await _userHelper.LogoutAsync(user);
        //        return Ok("eX01");
        //    }

        //    try
        //    {
        //        var prod = await _productHelper.AddProductAsync(model);
        //        return Ok(prod);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS DELETE"))
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

            Producto producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("ReparaKardex")]
        public async Task<ActionResult<Kardex>> ReparaKardex()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "KARDEX VER"))
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
                var kardex = await _productHelper.ReparaKardexAsync();
                return Ok(kardex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProdsDifKardex")]
        public async Task<ActionResult<Kardex>> GetProdsDifKardex()
        {
            string email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "KARDEX VER"))
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
                var result = await _productHelper.GetProdsDifKardex();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetKardex")]
        public async Task<ActionResult<Kardex>> GetKardex([FromBody] GetKardexViewModel model)
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
            if (!await _userHelper.IsAutorized(user.Rol, "KARDEX VER"))
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
                var kardex = await _productHelper.GetKardex(model);
                return Ok(kardex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetAllStoresKardex")]
        public async Task<ActionResult<Kardex>> GetAllStoresKardex([FromBody] GetKardexViewModel model)
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
            if (!await _userHelper.IsAutorized(user.Rol, "KARDEX VER"))
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
                var kardex = await _productHelper.GetAllStoresKardex(model);
                return Ok(kardex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("getProductsListM")]
        [Authorize]
        //[Route("getProductsListM")]
        public async Task<ActionResult<IEnumerable<GetProductslistEntity>>> getProductsListM([FromBody] GetProductslistViewModels model)

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
                var result = await _productHelper.GetProductslistM
                    (model.StoreId,
                    model.TipoNegocio,
                    model.familia);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(int? almacen, int? tipoNegocio, int? familia)
        //{
        //    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (email == null)
        //    {
        //        return Unauthorized();
        //    }

        //    User user = await _userHelper.GetUserByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    if (user.IsDefaultPass)
        //    {
        //        return Ok(user);
        //    }

        //    string token = HttpContext.Request.Headers["Authorization"].ToString();
        //    token = token["Bearer ".Length..].Trim();
        //    if (user.UserSession.UserToken != token)
        //    {
        //        await _userHelper.LogoutAsync(user);
        //        return Ok("eX01");
        //    }

        //    try
        //    {
        //        var products = await Task.Run(() => _productHelper.GetProducts(almacen, tipoNegocio, familia));
        //        var viewModel = new GetProductslistViewModels
        //        {
        //            Products = products
        //        };

        //        return View(viewModel);
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(500, $"Error al obtener productos: {ex.Message}");
        //    }
        //}

        [HttpPost]
        [Route("UpdaterecallProductId")]
        public async Task<ActionResult<IEnumerable<ProductsRecal>>> UpdaterecallProductId([FromBody] UpdateProductRecallViewModel model)
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
                var updatedProduct = await _productHelper.UpdateProductRecallAsync(
                    model.Id,
                    model.StoreId,
                    model.Porcentaje,
                    model.ActualizarVentaDetalle, 
                    model.ActualizarVentaMayor);  

                if (updatedProduct == null)
                {
                    return NotFound($"No se encontró ningún producto para el storeId {model.Id}");
                }

                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el producto con el storeId {model.Id}: {ex.Message}");
            }
        }





    }
}

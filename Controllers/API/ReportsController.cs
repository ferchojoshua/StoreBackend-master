using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Store.Entities;
using Store.Helpers.ReportHelper;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ReportsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IReportsHelper _reportHelper;

        public ReportsController(IUserHelper userHelper, IReportsHelper reportHelper)
        {
            _userHelper = userHelper;
            _reportHelper = reportHelper;
        }

        [HttpPost("GetMasterVentas")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetMasterVentas(
            [FromBody] MasterVentasViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "MASTER VENTAS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportMasterVentas(model);
                return Ok(result.OrderByDescending(r => r.FechaVenta));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetCuentasXCobrar")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetCuentasXCobrar(
            [FromBody] CuentasXCobrarViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "CUENTASXCOBRAR VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportCuentasXCobrar(model);
                return Ok(result.OrderBy(r => r.Client.NombreCliente));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetProdVendidos")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetArtVendidos(
            [FromBody] ArtVendidosViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "PRODVENDIDOS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportArticulosVendidos(model);
                return Ok(result.OrderBy(x => x.Producto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetCierreDiario")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetCierreDiario(
            [FromBody] CierreDiarioViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "CIERREDIARIO VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportCierreDiario(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetCajaChica")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetCajaChica(
            [FromBody] CajaChicaViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "CAJACHICA VER"))
            {
                return Unauthorized();
            }
            try
            {
                var result = await _reportHelper.ReportCajaChica(model);
                return Ok(result.OrderBy(x => x.Fecha));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetProdNoVendidos")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetProdNoVendidos(
            [FromBody] ArtNoVendidosViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "PRODNOVENDIDOS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportArticulosNoVendidos(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetIngresos")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetIngresos(
            [FromBody] IngresosViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "INGRESOS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportIngresos(model);
                return Ok(result.OrderByDescending(r => r.FechaAbono));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetCompras")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetCompras(
            [FromBody] ComprasViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "REPORTECOMPRAS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportCompras(model);
                return Ok(result.OrderByDescending(r => r.Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetTrasladoInventario")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetTrasladoInventario(
            [FromBody] TrasladoInventarioViewModel model
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
            if (!await _userHelper.IsAutorized(user.Rol, "REPORTETRASLADOS VER"))
            {
                return Unauthorized();
            }

            try
            {
                var result = await _reportHelper.ReportproductMovments(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("GetProductosInventario")]
            public async Task<ActionResult<IEnumerable<ProductosInventario>>> GetProductosInventario([FromBody] ProductosInventarioViewModel model)
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
                if (!await _userHelper.IsAutorized(user.Rol, "INVENTARIO VER"))
                {
                    return Unauthorized();
                }

                try
                {


                    var result = await _reportHelper.GetProductosInventarioAsync(model.ProductID, model.StoreID, model.TipoNegocioID, model.FamiliaID,model.showststore, model.OmitirStock);

                    //int? productID = 26882;  
                    //int? storeID = 1;
                    //int? tipoNegocioID = 4;

                    //var requestModel = new ProductosInventarioViewModel
                    //{
                    //    ProductID = productID,
                    //    StoreID = storeID,
                    //    TipoNegocioID = tipoNegocioID
                    //};

                    //var result = await _reportHelper.GetProductosInventarioAsync(int productID, int storeID, int tipoNegocioID);

                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }

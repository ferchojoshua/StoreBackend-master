using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Store.Entities;
using Store.Helpers.FacturacionHelper;
using Store.Helpers.User;
using Store.Hubs;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class FacturationController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IFacturationHelper _facturation;
        private readonly IHubContext<NewFacturaHub> _hubContext;

        public FacturationController(
            IUserHelper userHelper,
            IFacturationHelper facturation,
            IHubContext<NewFacturaHub> hubContext
        )
        {
            _userHelper = userHelper;
            _facturation = facturation;
            _hubContext = hubContext;
        }

        [HttpGet("GetFacturationsUncancelled/{id}")]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetFacturationsUncancelled(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
            {
                return Unauthorized();
            }
            try
            {
                var facturacions = await _facturation.GetFacturacionAsync(id);
                return Ok(facturacions.OrderByDescending(s => s.FechaVenta));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        //[HttpPut("UpdateFacturacionAsync")]
        //public async Task<ActionResult<Facturacion>> UpdateFacturacion([FromBody] AddupdateViewModel model)
        //{
        //    // Obtener el email del usuario autenticado
        //    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return Unauthorized("Usuario no autenticado.");
        //    }

        //    User user = await _userHelper.GetUserByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return Unauthorized("Usuario no encontrado.");
        //    }


        //    if (user.IsDefaultPass)
        //    {
        //        return Ok(user);
        //    }

        //    string token = HttpContext.Request.Headers["Authorization"];
        //    token = token["Bearer ".Length..].Trim();
        //    if (user.UserSession.UserToken != token)
        //    {
        //        await _userHelper.LogoutAsync(user);
        //        return Ok("eX01");
        //    }


        //    if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
        //    {
        //        return Unauthorized();
        //    }

        //    try
        //    {
        //        // Llamar a la lógica de actualización de facturación, pasando el `id` y el `model`
        //        var updatedFacturacion = await _facturation.UpdateFacturacionAsync(model, user);

        //        // Devolver la facturación actualizada
        //        return Ok(updatedFacturacion);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error al actualizar la facturación: {ex.Message}");
        //    }
        //}



        //[HttpGet("GetProformasFacturacion")]
        //public async Task<ActionResult<IEnumerable<Facturacion>>> GetProformasFacturacion()
        //{
        //    string email = User.Claims
        //        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
        //        .Value;
        //    User user = await _userHelper.GetUserByEmailAsync(email);
        //    if (user.IsDefaultPass)
        //    {
        //        return Ok(user);
        //    }
        //    string token = HttpContext.Request.Headers["Authorization"];
        //    token = token["Bearer ".Length..].Trim();
        //    if (user.UserSession.UserToken != token)
        //    {
        //        await _userHelper.LogoutAsync(user);
        //        return Ok("eX01");
        //    }
        //    if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
        //    {
        //        return Unauthorized();
        //    }
        //    try
        //    {
        //        var facturacions = await _facturation.GetProformasFacturacionAsync();
        //        return Ok(facturacions.OrderByDescending(s => s.FechaVenta));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}





        [HttpGet("GetFactCancelled/{id}")]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetFactCancelled(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
            {
                return Unauthorized();
            }

            var facturacions = await _facturation.GetCancelledFacturacionAsync(id);
            return Ok(facturacions.OrderByDescending(s => s.FechaVenta));
        }

        [HttpGet("GetTipoPagos")]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetTipoPagos()
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

            var facturacions = await _facturation.GetTipoPagoAsync();
            return Ok(facturacions);
        }

        [HttpGet("GetReprintSale/{id}")]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetReprintSale(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
            {
                return Unauthorized();
            }

            var sale = await _facturation.GetReprintBillAsync(id);
            return Ok(sale);
        }

        [HttpGet("GetFactAnulated/{id}")]
        public async Task<ActionResult<IEnumerable<Facturacion>>> GetFactAnulated(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
            {
                return Unauthorized();
            }

            var facturacions = await _facturation.GetAnulatedFacturacionAsync(id);
            return Ok(facturacions.OrderByDescending(s => s.FechaAnulacion));
        }

        [HttpPost]
        public async Task<ActionResult<Sales>> Addfactura([FromBody] AddFacturacionViewModel model)
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

            if (!await _userHelper.IsAutorized(user.Rol, "SALES FACTURACION"))
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
                var sale = await _facturation.AddFacturacionAsync(model, user);
                await _hubContext.Clients.All.SendAsync("factListUpdate");
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Paidfactura")]
        public async Task<ActionResult<Sales>> Paidfactura([FromBody] PayFactViewModel model)
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

            if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
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
                var sale = await _facturation.PayFacturaAsync(model, user);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteFactura/{id}")]
        public async Task<ActionResult<Sales>> DeleteFactura(int id)
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

            if (!await _userHelper.IsAutorized(user.Rol, "SALES DELETE"))
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
                var sale = await _facturation.DeleteFacturacionAsync(id, user);
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

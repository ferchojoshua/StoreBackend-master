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
                await _userHelper.LogoutAsync();
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
                await _userHelper.LogoutAsync();
                return Ok("eX01");
            }
            if (!await _userHelper.IsAutorized(user.Rol, "SALES CAJA"))
            {
                return Unauthorized();
            }

            var facturacions = await _facturation.GetCancelledFacturacionAsync(id);
            return Ok(facturacions.OrderByDescending(s => s.FechaVenta));
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
                await _userHelper.LogoutAsync();
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
                await _userHelper.LogoutAsync();
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
                await _userHelper.LogoutAsync();
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
                await _userHelper.LogoutAsync();
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
                await _userHelper.LogoutAsync();
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

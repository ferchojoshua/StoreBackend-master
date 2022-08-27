using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Store.Entities;
using Store.Helpers.SalesHelper;
using Store.Helpers.User;
using Store.Models.ViewModels;
using StoreBackend.Hubs;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly ISalesService _salesService;
        private readonly IHubContext<NewSalehub> _hubContext;

        public SalesController(
            IUserHelper userHelper,
            ISalesService salesService,
            IHubContext<NewSalehub> hubContext
        )
        {
            _userHelper = userHelper;
            _salesService = salesService;
            _hubContext = hubContext;
        }

        [HttpGet("GetContadoSalesByStore/{id}")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetContadoSalesByStore(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var sales = await _salesService.GetContadoSalesByStoreAsync(id);
            return Ok(sales.OrderByDescending(s => s.FechaVenta));
        }

        [HttpGet("GetCreditoSalesByStore/{id}")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetCreditoSalesByStore(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var sales = await _salesService.GetCreditoSalesByStoreAsync(id);
            return Ok(sales.OrderByDescending(s => s.FechaVenta));
        }

        [HttpGet("GetAnulatedSalesByStore/{id}")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetAnulatedSalesByStore(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var sales = await _salesService.GetAnulatedSalesByStoreAsync(id);
            return Ok(sales.OrderByDescending(s => s.FechaVenta));
        }

        [HttpGet("GetPaysBySaleId/{id}")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetPaysBySaleId(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var quotes = await _salesService.GetQuoteListAsync(id);
            return Ok(quotes.OrderByDescending(q => q.FechaAbono));
        }

        [HttpGet("GetSalesUncanceledByClient/{id}")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetSalesUncanceledByClient(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var quotes = await _salesService.GetSalesUncanceledByClientAsync(id);
            return Ok(quotes.OrderByDescending(q => q.Sale.FechaVenta));
        }

        [HttpPost]
        public async Task<ActionResult<Sales>> AddSale([FromBody] AddSaleViewModel model)
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

            if (!await _userHelper.IsAutorized(user.Rol, "SALES CREATE"))
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
                var sale = await _salesService.AddSaleAsync(model, user);
                await _hubContext.Clients.All.SendAsync("saleUpdate");
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAbono")]
        public async Task<ActionResult<Sales>> AddAbono([FromBody] AddAbonoViewModel model)
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

            if (!await _userHelper.IsAutorized(user.Rol, "PAGO CREATE"))
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
                var abono = await _salesService.AddAbonoAsync(model, user);
                return Ok(abono);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAbonoEspecifico")]
        public async Task<ActionResult<Sales>> AddAbonoEspecifico(
            [FromBody] AddAbonoEspecificoViewModel model
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

            if (!await _userHelper.IsAutorized(user.Rol, "PAGO ESPECIFICO CREATE"))
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
                var abono = await _salesService.AddAbonoEspecificoAsync(model, user);
                return Ok(abono);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AnularVenta/{id}")]
        public async Task<ActionResult<Sales>> AnularVenta(int id)
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
                var sale = await _salesService.AnularSaleAsync(id, user);
                if (sale == null)
                {
                    return NoContent();
                }
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AnularVentaParcial")]
        public async Task<ActionResult<Sales>> AnularVentaParcial(
            [FromBody] EditSaleViewModel model
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
                var sale = await _salesService.AnularSaleParcialAsync(model, user);
                if (sale == null)
                {
                    return NoContent();
                }
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

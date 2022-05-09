using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities;
using Store.Helpers.SalesHelper;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly ISalesService _salesService;

        public SalesController(IUserHelper userHelper, ISalesService salesService)
        {
            _userHelper = userHelper;
            _salesService = salesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sales>>> GetSales()
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var sales = await _salesService.GetSalesListAsync();
            return Ok(sales.OrderByDescending(s => s.FechaVenta));
        }

        [HttpGet("GetPaysBySaleId/{id}")]
        public async Task<ActionResult<IEnumerable<Sales>>> GetPaysBySaleId(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "SALES VER"))
            {
                return Unauthorized();
            }

            var quotes = await _salesService.GetQuoteListAsync(id);
            return Ok(quotes.OrderByDescending(q => q.FechaAbono));
        }

        [HttpPost]
        public async Task<ActionResult<Sales>> AddSale([FromBody] AddSaleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

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

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

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
                var sale = await _salesService.AddAbonoAsync(model, user);
                return Ok(sale);
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

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "SALES UPDATE"))
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
        public async Task<ActionResult<Sales>> AnularVentaParcial([FromBody] EditSaleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }

            if (!await _userHelper.IsAutorized(user.Rol, "SALES UPDATE"))
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

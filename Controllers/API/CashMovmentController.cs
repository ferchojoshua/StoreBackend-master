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
    public class CashMovmentController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly ICashMovmentService _cashService;

        public CashMovmentController(IUserHelper userHelper, ICashMovmentService cashService)
        {
            _userHelper = userHelper;
            _cashService = cashService;
        }

        [HttpGet("GetCashMovmentByStore/{id}")]
        public async Task<ActionResult<IEnumerable<CajaMovment>>> GetCashMovmentByStore(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "CAJA VER"))
            {
                return Unauthorized();
            }

            var sales = await _cashService.GetCashMovmentByStoreAsync(id);
            return Ok(sales.OrderByDescending(s => s.Id));
        }
    }
}

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities;
using Store.Helpers.ClientService;
using Store.Helpers.SalesHelper;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IDashboardService _dashboardService;

        public DashboardController(IUserHelper userHelper, IDashboardService dashboardService)
        {
            _userHelper = userHelper;
            _dashboardService = dashboardService;
        }

        [HttpGet("GetSalesMonthByStore/{id}")]
        public async Task<ActionResult<decimal>> GetSalesMonthByStore(int id)
        {
            try
            {
                return Ok(await _dashboardService.GetSalesMonthByStoreAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSalesRecupMonth/{id}")]
        public async Task<ActionResult<decimal>> GetSalesRecupMonth(int id)
        {
            try
            {
                return Ok(await _dashboardService.GetSalesRecupMonthAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSalesWeekByStore/{id}")]
        public async Task<ActionResult<decimal>> GetSalesWeekByStore(int id)
        {
            try
            {
                return Ok(await _dashboardService.GetSalesWeekByStoreAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNewClientsByStore/{id}")]
        public async Task<ActionResult<decimal>> GetNewClientsByStore(int id)
        {
            try
            {
                return Ok(await _dashboardService.GetNewClientsByStoreAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSalesByDate/{id}")]
        public async Task<ActionResult<decimal>> GetSalesByDate(int id)
        {
            try
            {
                return Ok(await _dashboardService.GetSalesByDateAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

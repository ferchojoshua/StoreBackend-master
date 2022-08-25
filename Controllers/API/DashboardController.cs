using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities;
using Store.Helpers.ClientService;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        // private readonly IUserHelper _userHelper;
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            // _userHelper = userHelper;
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
                var data = await _dashboardService.GetSalesByDateAsync(id);
                return Ok(data.OrderBy(d => d.Fecha));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetVisitedClientsByStore/{id}")]
        public async Task<ActionResult<int>> GetVisitedClientsByStore(int id)
        {
            try
            {
                return Ok(await _dashboardService.GetVisitedClientsByStoreAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetClientsByLocationAndStore/{id}")]
        public async Task<ActionResult<int>> GetClientsByLocationAndStore(int id)
        {
            try
            {
                var ClientList = await _dashboardService.GetClientsByLocationAndStoreAsync(id);
                var result = ClientList
                    .GroupBy(cl => cl.Community.Municipality)
                    .Select(x => new { Location = x.Key.Name, Contador = x.Count() });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSalesByTNAndStore/{id}")]
        public async Task<ActionResult<int>> GetSalesByTNAndStore(int id)
        {
            List<Producto> prodList = new();
            try
            {
                var saleList = await _dashboardService.GetSalesByTNAndStoreAsync(id);
                foreach (var sale in saleList)
                {
                    foreach (var detail in sale.SaleDetails)
                    {
                        prodList.Add(detail.Product);
                    }
                }
                var result = prodList
                    .GroupBy(p => p.TipoNegocio)
                    .Select(x => new { Tn = x.Key.Description, Contador = x.Count() });
                return Ok(result.OrderBy(r => r.Tn));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

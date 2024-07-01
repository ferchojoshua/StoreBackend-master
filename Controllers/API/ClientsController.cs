using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities;
using Store.Helpers.ClientService;
using Store.Helpers.User;
using Store.Models.ViewModels;
namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsHelper _clientsHelper;
        private readonly IUserHelper _userHelper;

        public ClientsController(IClientsHelper clientsHelper, IUserHelper userHelper)
        {
            _clientsHelper = clientsHelper;
            _userHelper = userHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "CLIENTS VER"))
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
                var clientList = await _clientsHelper.GetClientListAsync();
                return Ok(clientList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            string email =
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user.IsDefaultPass)
            {
                return Ok(user);
            }
            if (!await _userHelper.IsAutorized(user.Rol, "CLIENTS VER"))
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
                var client = await _clientsHelper.GetClientAsync(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Client>> AddClient([FromBody] AddClientViewModel model)
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
            if (!await _userHelper.IsAutorized(user.Rol, "CLIENTS CREATE"))
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
                var client = await _clientsHelper.AddClientAsync(model, user);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateClient")]
        public async Task<ActionResult<Client>> UpdateClient([FromBody] UpdateClientViewModel model)
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
            if (!await _userHelper.IsAutorized(user.Rol, "CLIENTS UPDATE"))
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
                var client = await _clientsHelper.UpdateClientAsync(model, user);
                if (client == null)
                {
                    return NoContent();
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteClient/{id}")]
        public async Task<ActionResult<Community>> DeleteClient(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "CLIENTS DELETE"))
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
                var client = await _clientsHelper.DeleteClientAsync(id);
                if (client == null)
                {
                    return NoContent();
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetRoute")]
        public async Task<ActionResult<Community>> GetRoute(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "CLIENTS DELETE"))
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
                var client = await _clientsHelper.DeleteClientAsync(id);
                if (client == null)
                {
                    return NoContent();
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}

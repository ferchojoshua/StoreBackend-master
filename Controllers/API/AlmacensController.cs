using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.StoreService;
using Store.Helpers.User;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacensController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IStoreHelper _storeHelper;
        private readonly DataContext _context;

        public AlmacensController(
            DataContext context,
            IUserHelper userHelper,
            IStoreHelper storeHelper
        )
        {
            _userHelper = userHelper;
            _storeHelper = storeHelper;
            _context = context;
        }

        // GET: api/Almacens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlmacenResponse>>> GetAlmacen()
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
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS VER"))
            {
                return Unauthorized();
            }
            List<AlmacenResponse> aR = new();
            var almacen = await _context.Almacen.ToListAsync();

            foreach (var item in almacen)
            {
                var racks = await _context.Racks.Where(r => r.Almacen == item).ToListAsync();
                aR.Add(new AlmacenResponse { Almacen = item, RacksNumber = racks.Count, });
            }
            return Ok(aR.OrderBy(a => a.Almacen.Name));
        }

        [HttpGet("GetStoresByUser")]
        public async Task<ActionResult<IEnumerable<Almacen>>> GetStoresByUser()
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
            try
            {
                var stores = user.StoreAccess;
                return Ok(stores);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Almacen>> GetAlmacen(int id)
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

            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS VER"))
            {
                return Unauthorized();
            }
            var almacen = await _context.Almacen.FirstOrDefaultAsync(p => p.Id == id);

            if (almacen == null)
            {
                return NotFound();
            }

            return almacen;
        }

        [HttpPost]
        [Route("UpdateAlmacen")]
        public async Task<IActionResult> UpdateAlmacen([FromBody] Almacen model)
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
                var store = await _storeHelper.UpdateStoreAsync(model);
                if (store == null)
                {
                    return NotFound(store);
                }
                return Ok(store);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Almacens
        [HttpPost]
        public async Task<ActionResult<Almacen>> PostAlmacen([FromBody] Almacen almacen)
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
            _context.Almacen.Add(almacen);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAlmacen", new { id = almacen.Id }, almacen);
        }

        // DELETE: api/Almacens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlmacen(int id)
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
            try
            {
                var almacen = await _context.Almacen.FindAsync(id);
                if (almacen == null)
                {
                    return BadRequest("Almacen no Existe");
                }
                _context.Almacen.Remove(almacen);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        private bool AlmacenExists(int id)
        {
            return _context.Almacen.Any(e => e.Id == id);
        }

        [HttpGet("GetRacksByStore/{id}")]
        public async Task<ActionResult<IEnumerable<Rack>>> GetRacksByStore(int id)
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
            return await _context.Racks.Where(r => r.Almacen.Id == id).ToListAsync();
        }

        [HttpPost("AddRacksToStore")]
        public async Task<ActionResult<Almacen>> AddRacksToStore([FromBody] AddRackViewModel model)
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
            Rack rack =
                new()
                {
                    Almacen = await _context.Almacen.FirstOrDefaultAsync(
                        p => p.Id == model.AlmacenId
                    ),
                    Description = model.Description
                };

            _context.Racks.Add(rack);
            await _context.SaveChangesAsync();
            return Ok(rack);
        }

        [HttpGet("GetRackById/{id}")]
        public async Task<ActionResult<Rack>> GetRackById(int id)
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
            var rack = await _context.Racks.FirstOrDefaultAsync(p => p.Id == id);

            if (rack == null)
            {
                return NotFound();
            }
            return rack;
        }

        [HttpPost("UpdateRack")]
        public async Task<IActionResult> UpdateRack([FromBody] UpdateRackViewModel model)
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
            Rack r = await _context.Racks.FirstOrDefaultAsync(rac => rac.Id == model.Id);
            r.Description = model.Description;
            _context.Entry(r).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlmacenExists(r.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/Almacens/5
        [HttpDelete("DeleteRack/{id}")]
        public async Task<IActionResult> DeleteRack(int id)
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
            if (!await _userHelper.IsAutorized(user.Rol, "MISCELANEOS DELETE"))
            {
                return Unauthorized();
            }
            var rack = await _context.Racks.FindAsync(id);
            if (rack == null)
            {
                return NotFound();
            }

            _context.Racks.Remove(rack);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

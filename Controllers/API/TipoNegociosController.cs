#nullable disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Helpers.User;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoNegociosController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public TipoNegociosController(DataContext context, IUserHelper userHelper)
        {
            _userHelper = userHelper;
            _context = context;
        }

        // GET: api/TipoNegocios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoNegocio>>> GetTipoNegocios()
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

            return await _context.TipoNegocios
                .Include(tn => tn.Familias)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        // GET: api/TipoNegocios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoNegocio>> GetTipoNegocio(int id)
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
            TipoNegocio tipoNegocio = await _context.TipoNegocios.FindAsync(id);

            if (tipoNegocio == null)
            {
                return NotFound();
            }

            return tipoNegocio;
        }

        // PUT: api/TipoNegocios/
        [HttpPost]
        [Route("UpdateTipoNegocio")]
        public async Task<IActionResult> PutTipoNegocio([FromBody] TipoNegocio tipoNegocio)
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
            _context.Entry(tipoNegocio).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoNegocioExists(tipoNegocio.Id))
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

        // POST: api/TipoNegocios
        [HttpPost]
        public async Task<ActionResult<TipoNegocio>> PostTipoNegocio(
            [FromBody] TipoNegocio tipoNegocio
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
            _context.TipoNegocios.Add(tipoNegocio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoNegocio", new { id = tipoNegocio.Id }, tipoNegocio);
        }

        // DELETE: api/TipoNegocios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoNegocio(int id)
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
            TipoNegocio tipoNegocio = await _context.TipoNegocios.FindAsync(id);
            if (tipoNegocio == null)
            {
                return NotFound();
            }

            _context.TipoNegocios.Remove(tipoNegocio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoNegocioExists(int id)
        {
            return _context.TipoNegocios.Any(e => e.Id == id);
        }

        [HttpGet("GetFamiliasByTN/{id}")]
        public async Task<ActionResult<IEnumerable<Familia>>> GetFamiliasByTN(int id)
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
            var data = await _context.TipoNegocios
                .Include(tn => tn.Familias)
                .FirstOrDefaultAsync(tn => tn.Id == id);
            return data.Familias.OrderBy(f => f.Description).ToList();
        }

        [HttpPost("AddFamiliaToTipoNegocio")]
        public async Task<ActionResult<Familia>> AddFamiliaToTipoNegocio(
            [FromBody] AddFamiliaViewModel model
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
            Familia f = new() { Description = model.Description };
            await _context.SaveChangesAsync();
            TipoNegocio tN = new();
            tN = await _context.TipoNegocios
                .Include(tn => tn.Familias)
                .FirstOrDefaultAsync(p => p.Id == model.IdTipoNegocio);

            tN.Familias.Add(f);
            _context.Entry(tN).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(tN.Familias);
        }

        [HttpGet("GetFamiliaById/{id}")]
        public async Task<ActionResult<Familia>> GetFamiliaById(int id)
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
            Familia familia = await _context.Familias.FindAsync(id);

            if (familia == null)
            {
                return NotFound();
            }

            return familia;
        }

        [HttpPost("UpdateFamilia")]
        public async Task<IActionResult> UpdateFamilia([FromBody] Familia model)
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
            Familia f = await _context.Familias.FirstOrDefaultAsync(fam => fam.Id == model.Id);
            if (f == null)
            {
                return NotFound();
            }
            f.Description = model.Description;
            _context.Entry(f).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpPost]
        [Route("DeleteFamilia/{id}")]
        public async Task<IActionResult> DeleteFamilia(int id)
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
            var fam = await _context.Familias.FindAsync(id);
            if (fam == null)
            {
                return NotFound();
            }

            _context.Familias.Remove(fam);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

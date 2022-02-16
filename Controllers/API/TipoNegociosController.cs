#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoNegociosController : ControllerBase
    {
        private readonly DataContext _context;

        public TipoNegociosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TipoNegocios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoNegocio>>> GetTipoNegocios()
        {
            return await _context.TipoNegocios.OrderByDescending(t => t.Id).ToListAsync();
        }

        // GET: api/TipoNegocios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoNegocio>> GetTipoNegocio(int id)
        {
            TipoNegocio tipoNegocio = await _context.TipoNegocios.FindAsync(id);

            if (tipoNegocio == null)
            {
                return NotFound();
            }

            return tipoNegocio;
        }

        // PUT: api/TipoNegocios/
        [HttpPut]
        [Route("UpdateTipoNegocio")]
        public async Task<IActionResult> PutTipoNegocio([FromBody] TipoNegocio tipoNegocio)
        {
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
        public async Task<ActionResult<TipoNegocio>> PostTipoNegocio(TipoNegocio tipoNegocio)
        {
            _context.TipoNegocios.Add(tipoNegocio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoNegocio", new { id = tipoNegocio.Id }, tipoNegocio);
        }

        // DELETE: api/TipoNegocios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoNegocio(int id)
        {
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
    }
}

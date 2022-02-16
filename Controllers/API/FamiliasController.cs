#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamiliasController : ControllerBase
    {
        private readonly DataContext _context;

        public FamiliasController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Familias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Familia>>> GetFamilias()
        {
            return await _context.Familias.ToListAsync();
        }

        // GET: api/Familias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Familia>> GetFamilia(int id)
        {
            Familia familia = await _context.Familias.FindAsync(id);

            if (familia == null)
            {
                return NotFound();
            }

            return familia;
        }

        [HttpPut]
        [Route("UpdateFamilia")]
        public async Task<IActionResult> UpdateFamilia([FromBody] Familia request)
        {
            _context.Entry(request).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamiliaExists(request.Id))
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


        [HttpPost]
        public async Task<ActionResult<Familia>> PostFamilia(Familia familia)
        {
            _context.Familias.Add(familia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamilia", new { id = familia.Id }, familia);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamilia(int id)
        {
            Familia familia = await _context.Familias.FindAsync(id);
            if (familia == null)
            {
                return NotFound();
            }

            _context.Familias.Remove(familia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FamiliaExists(int id)
        {
            return _context.Familias.Any(e => e.Id == id);
        }
    }
}

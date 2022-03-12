#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;
using Store.Models.Responses;
using Store.Models.ViewModels;

namespace Store.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacensController : ControllerBase
    {
        private readonly DataContext _context;

        public AlmacensController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Almacens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlmacenResponse>>> GetAlmacen()
        {
            List<AlmacenResponse> aR = new();                
            var almacen = await _context.Almacen.ToListAsync();

            foreach (var item in almacen)
            {
                var racks = await _context.Racks.Where(r => r.Almacen == item).ToListAsync();
                aR.Add(new AlmacenResponse
                {
                    Almacen = item,
                    RacksNumber = racks.Count,
                });               
            }
            return Ok(aR);
        }

        // GET: api/Almacens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Almacen>> GetAlmacen(int id)
        {
            var almacen = await _context.Almacen.FirstOrDefaultAsync(p => p.Id == id);

            if (almacen == null)
            {
                return NotFound();
            }

            return almacen;
        }

        // PUT: api/Almacens/
        [HttpPut]
        [Route("UpdateAlmacen")]
        public async Task<IActionResult> UpdateAlmacen([FromBody]Almacen almacen)
        {            
            _context.Entry(almacen).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlmacenExists(almacen.Id))
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

        // POST: api/Almacens
        [HttpPost]
        public async Task<ActionResult<Almacen>> PostAlmacen(Almacen almacen)
        {
            _context.Almacen.Add(almacen);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAlmacen", new { id = almacen.Id }, almacen);
        }

        // DELETE: api/Almacens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlmacen(int id)
        {
            var almacen = await _context.Almacen.FindAsync(id);
            if (almacen == null)
            {
                return NotFound();
            }

            _context.Almacen.Remove(almacen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlmacenExists(int id)
        {
            return _context.Almacen.Any(e => e.Id == id);
        }

        [HttpGet("GetRacksByStore/{id}")]
        public async Task<ActionResult<IEnumerable<Rack>>> GetRacksByStore(int id)
        {
            return await _context.Racks.Where(r => r.Almacen.Id == id).ToListAsync();
        }

        [HttpPost("AddRacksToStore")]
        public async Task<ActionResult<Almacen>> AddRacksToStore([FromBody]AddRackViewModel model)
        {
            Rack rack = new();
            rack.Almacen = await _context.Almacen.FirstOrDefaultAsync(p => p.Id == model.AlmacenId);
            rack.Description = model.Description;

            _context.Racks.Add(rack);
            await _context.SaveChangesAsync();
            return Ok(rack);
        }

        [HttpGet("GetRackById/{id}")]
        public async Task<ActionResult<Rack>> GetRackById(int id)
        {
            var rack = await _context.Racks.FirstOrDefaultAsync(p => p.Id == id);

            if (rack == null)
            {
                return NotFound();
            }
            return rack;
        }

        [HttpPut("UpdateRack")]
        public async Task<IActionResult> UpdateRack([FromBody] UpdateRackViewModel model)
        {
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

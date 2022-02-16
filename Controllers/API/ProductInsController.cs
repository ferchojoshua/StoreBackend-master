#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductInsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ProductIns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductIn>>> GetProductIns()
        {
            return await _context.ProductIns.Include(p => p.ProductInDetails).Include(p => p.Provider).ToListAsync();
        }

        // GET: api/ProductIns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductIn>> GetProductIn(int id)
        {
            var productIn = await _context.ProductIns.FindAsync(id);

            if (productIn == null)
            {
                return NotFound();
            }

            return productIn;
        }

        // PUT: api/ProductIns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductIn(int id, ProductIn productIn)
        {
            if (id != productIn.Id)
            {
                return BadRequest();
            }

            _context.Entry(productIn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductInExists(id))
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

        // POST: api/ProductIns
        [HttpPost]
        public async Task<ActionResult<ProductIn>> PostProductIn(ProductIn productIn)
        {
            _context.ProductIns.Add(productIn);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductIn", new { id = productIn.Id }, productIn);
        }

        // DELETE: api/ProductIns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductIn(int id)
        {
            var productIn = await _context.ProductIns.FindAsync(id);
            if (productIn == null)
            {
                return NotFound();
            }

            _context.ProductIns.Remove(productIn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductInExists(int id)
        {
            return _context.ProductIns.Any(e => e.Id == id);
        }
    }
}

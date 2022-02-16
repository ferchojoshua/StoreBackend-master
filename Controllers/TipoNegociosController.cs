#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Entities;

namespace Store.Controllers
{
    public class TipoNegociosController : Controller
    {
        private readonly DataContext _context;

        public TipoNegociosController(DataContext context)
        {
            _context = context;
        }

        // GET: TipoNegocios
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoNegocios.ToListAsync());
        }

        // GET: TipoNegocios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoNegocio tipoNegocio = await _context.TipoNegocios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoNegocio == null)
            {
                return NotFound();
            }

            return View(tipoNegocio);
        }

        // GET: TipoNegocios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoNegocios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description")] TipoNegocio tipoNegocio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoNegocio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoNegocio);
        }

        // GET: TipoNegocios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoNegocio tipoNegocio = await _context.TipoNegocios.FindAsync(id);
            if (tipoNegocio == null)
            {
                return NotFound();
            }
            return View(tipoNegocio);
        }

        // POST: TipoNegocios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description")] TipoNegocio tipoNegocio)
        {
            if (id != tipoNegocio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoNegocio);
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
                return RedirectToAction(nameof(Index));
            }
            return View(tipoNegocio);
        }

        // GET: TipoNegocios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoNegocio tipoNegocio = await _context.TipoNegocios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoNegocio == null)
            {
                return NotFound();
            }

            return View(tipoNegocio);
        }

        // POST: TipoNegocios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TipoNegocio tipoNegocio = await _context.TipoNegocios.FindAsync(id);
            _context.TipoNegocios.Remove(tipoNegocio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoNegocioExists(int id)
        {
            return _context.TipoNegocios.Any(e => e.Id == id);
        }
    }
}

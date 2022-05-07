#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabaOne;

namespace LabaOne.Controllers
{
    public class CountriesVariantsController : Controller
    {
        private readonly DBFinalContext _context;

        public CountriesVariantsController(DBFinalContext context)
        {
            _context = context;
        }

        // GET: CountriesVariants
        public async Task<IActionResult> Index()
        {
            var virusBaseContext = _context.CountriesVariants.Include(c => c.Country).Include(c => c.Variant);
            return View(await virusBaseContext.ToListAsync());
        }

        // GET: CountriesVariants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countriesVariant = await _context.CountriesVariants
                .Include(c => c.Country)
                .Include(c => c.Variant)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (countriesVariant == null)
            {
                return NotFound();
            }

            return View(countriesVariant);
        }

        // GET: CountriesVariants/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id");
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id");
            return View();
        }

        // POST: CountriesVariants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryId,VariantId")] CountriesVariant countriesVariant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countriesVariant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", countriesVariant.CountryId);
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id", countriesVariant.VariantId);
            return View(countriesVariant);
        }

        // GET: CountriesVariants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countriesVariant = await _context.CountriesVariants.FindAsync(id);
            if (countriesVariant == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", countriesVariant.CountryId);
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id", countriesVariant.VariantId);
            return View(countriesVariant);
        }

        // POST: CountriesVariants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CountryId,VariantId")] CountriesVariant countriesVariant)
        {
            if (id != countriesVariant.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(countriesVariant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountriesVariantExists(countriesVariant.CountryId))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", countriesVariant.CountryId);
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id", countriesVariant.VariantId);
            return View(countriesVariant);
        }

        // GET: CountriesVariants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countriesVariant = await _context.CountriesVariants
                .Include(c => c.Country)
                .Include(c => c.Variant)
                .FirstOrDefaultAsync(m => m.CountryId == id);
            if (countriesVariant == null)
            {
                return NotFound();
            }

            return View(countriesVariant);
        }

        // POST: CountriesVariants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var countriesVariant = await _context.CountriesVariants.FindAsync(id);
            _context.CountriesVariants.Remove(countriesVariant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountriesVariantExists(int id)
        {
            return _context.CountriesVariants.Any(e => e.CountryId == id);
        }
    }
}

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
    public class SymptomsVariantsController : Controller
    {
        private readonly DBFinalContext _context;

        public SymptomsVariantsController(DBFinalContext context)
        {
            _context = context;
        }

        // GET: SymptomsVariants
        public async Task<IActionResult> Index()
        {
            var virusBaseContext = _context.SymptomsVariants.Include(s => s.Symptom).Include(s => s.Variant);
            return View(await virusBaseContext.ToListAsync());
        }

        // GET: SymptomsVariants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptomsVariant = await _context.SymptomsVariants
                .Include(s => s.Symptom)
                .Include(s => s.Variant)
                .FirstOrDefaultAsync(m => m.VariantId == id);
            if (symptomsVariant == null)
            {
                return NotFound();
            }

            return View(symptomsVariant);
        }

        // GET: SymptomsVariants/Create
        public IActionResult Create()
        {
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Id");
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id");
            return View();
        }

        // POST: SymptomsVariants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VariantId,SymptomId")] SymptomsVariant symptomsVariant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(symptomsVariant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Id", symptomsVariant.SymptomId);
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id", symptomsVariant.VariantId);
            return View(symptomsVariant);
        }

        // GET: SymptomsVariants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptomsVariant = await _context.SymptomsVariants.FindAsync(id);
            if (symptomsVariant == null)
            {
                return NotFound();
            }
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Id", symptomsVariant.SymptomId);
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id", symptomsVariant.VariantId);
            return View(symptomsVariant);
        }

        // POST: SymptomsVariants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VariantId,SymptomId")] SymptomsVariant symptomsVariant)
        {
            if (id != symptomsVariant.VariantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(symptomsVariant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymptomsVariantExists(symptomsVariant.VariantId))
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
            ViewData["SymptomId"] = new SelectList(_context.Symptoms, "Id", "Id", symptomsVariant.SymptomId);
            ViewData["VariantId"] = new SelectList(_context.Variants, "Id", "Id", symptomsVariant.VariantId);
            return View(symptomsVariant);
        }

        // GET: SymptomsVariants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptomsVariant = await _context.SymptomsVariants
                .Include(s => s.Symptom)
                .Include(s => s.Variant)
                .FirstOrDefaultAsync(m => m.VariantId == id);
            if (symptomsVariant == null)
            {
                return NotFound();
            }

            return View(symptomsVariant);
        }

        // POST: SymptomsVariants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var symptomsVariant = await _context.SymptomsVariants.FindAsync(id);
            _context.SymptomsVariants.Remove(symptomsVariant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SymptomsVariantExists(int id)
        {
            return _context.SymptomsVariants.Any(e => e.VariantId == id);
        }
    }
}

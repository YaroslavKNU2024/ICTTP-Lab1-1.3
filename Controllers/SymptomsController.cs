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
    public class SymptomsController : Controller
    {
        private readonly DBFinalContext _context;

        public SymptomsController(DBFinalContext context)
        {
            _context = context;
        }

        // GET: Symptoms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Symptoms.ToListAsync());
        }


        // GET: Symptoms/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
                return NotFound();

            var symptom1 = await _context.Symptoms.Include(x => x.Variants).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            var symptom = await _context.Symptoms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symptom == null)
                return NotFound();

            return View(symptom1);
        }

        // GET: Symptoms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Symptoms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SymptomName")] Symptom symptom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(symptom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(symptom);
        }

        // GET: Symptoms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptom = await _context.Symptoms.FindAsync(id);
            if (symptom == null)
            {
                return NotFound();
            }
            return View(symptom);
        }

        // POST: Symptoms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SymptomName")] Symptom symptom)
        {
            if (id != symptom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(symptom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymptomExists(symptom.Id))
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
            return View(symptom);
        }

        // GET: Symptoms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var symptom = await _context.Symptoms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symptom == null)
            {
                return NotFound();
            }

            return View(symptom);
        }

        // POST: Symptoms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var symptom = await _context.Symptoms.FindAsync(id);
            _context.Symptoms.Remove(symptom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SymptomExists(int id)
        {
            return _context.Symptoms.Any(e => e.Id == id);
        }
    }
}

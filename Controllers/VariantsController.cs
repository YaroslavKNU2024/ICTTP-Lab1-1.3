#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabaOne;
using LabaOne.Models;

namespace LabaOne.Controllers
{
    public class VariantsController : Controller
    {
        private readonly DBFinalContext _context;

        public VariantsController(DBFinalContext context)
        {
            _context = context;
        }

        // GET: Variants
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null || name == null)
            {
                var variants = _context.Variants.Include(x => x.Virus);
                ViewBag.VirusName = null;
                return View(await variants.ToListAsync());
            }
            //if (virusId == null)
            //{

                //var variants = await _context.Variants.ToListAsync();
                //for (int i = 0; i < variants.Count(); ++i)
                //{
                //    foreach (var v in _context.Viruses)
                //    {
                //        if (v.Id == variants[i].VirusId)
                //        {
                //            variants[i].Virus = v;
                //            break;
                //        }
                //    }
                //}
                //return View(variants);
            //}
            ViewBag.VirusId = id;
            ViewBag.VirusName= name;
            var variantsByVirus = _context.Variants.Where(x => x.VirusId == id).Include(x => x.Virus);
            return View(await variantsByVirus.ToListAsync());
        }

        // GET: Variants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var variant = await _context.Variants
                .FirstOrDefaultAsync(m => m.Id == id);
            var variantCountriesCopy = await _context.Variants.AsNoTracking().Include(x => x.Countries).FirstOrDefaultAsync(c => c.Id == id);
            var variantSymptomsCopy = await _context.Variants.AsNoTracking().Include(x => x.Symptoms).FirstOrDefaultAsync(c => c.Id == id);
            variant.Countries = variantCountriesCopy.Countries;
            variant.Symptoms = variantSymptomsCopy.Symptoms;
            if (variant == null)
                return NotFound();
            foreach  (var v in _context.Viruses) {
                if (v.Id == variant.VirusId) {
                    variant.Virus = v;
                    break;
                }
            }
            return View(variant);
        }

        // GET: /Create
        public IActionResult Create(int? virusId)
        {
            if (virusId == null)
            {
                ViewData["VirusId"] = new SelectList(_context.Viruses, "Id", "VirusName");
            }
            else
            {
                ViewBag.VirusId = virusId;
                ViewBag.VirusName = _context.Viruses.Where(d => d.Id == virusId).FirstOrDefault().VirusName;
            }
            return View();
        }

        // POST: /Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int virusId, [Bind("Id,VariantName,VariantOrigin,VariantDateDiscovered, VirusId")] Variant variant)
        {
            variant.VirusId = virusId;
            variant.Virus = await _context.Viruses.FindAsync(variant.VirusId);
            variant.Virus.Group = await _context.VirusGroups.FindAsync(variant.Virus.GroupId);
            ModelState.ClearValidationState(nameof(variant.Virus));
            ModelState.ClearValidationState(nameof(variant.Virus.Group));
            TryValidateModel(variant.Virus, nameof(variant.Virus));
            TryValidateModel(variant.Virus.Group, nameof(variant.Virus.Group));
            if (ModelState.IsValid)
            {
                _context.Add(variant);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Variants", new { id = virusId, name = _context.Viruses.Where(c => c.Id == virusId).FirstOrDefault().VirusName });
            }
            //ViewData["VirusId"] = new SelectList(_context.Viruss, "Id", "VirusName", variant.VirusId);
            //return View(variant);
            return RedirectToAction("Index", "Variants", new { id = virusId, name = _context.Viruses.Where(c => c.Id == virusId).FirstOrDefault().VirusName });

            //if (ModelState.IsValid)
            //{
            //    _context.Add(variant);
            //    //vr = variant;
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(variant);
        }


        public async Task<IActionResult> Edit(int? id, int? virusId)
        {
            if (id == null)
                return NotFound();

            var variant = await _context.Variants.Include(x => x.Countries).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (variant == null)// && variantSymptomsCopy == null)
                return NotFound();


            if (virusId != null)
            {
                ViewBag.VirusName = _context.Viruses.Where(f => f.Id == variant.VirusId).FirstOrDefault().VirusName;
                ViewBag.VirusId = virusId;
            }
            else
            {
                ViewBag.GroupName = null;
                ViewData["VirusId"] = new SelectList(_context.Viruses, "Id", "VirusName", virusId);
            }



            ViewBag.Countries = new MultiSelectList(_context.Countries, "Id", "CountryName");
            ViewBag.Symptoms = new MultiSelectList(_context.Symptoms, "Id", "SymptomName");
            var variantEdit = new VariantsEdit
            {
                Id = variant.Id,
                VariantName = variant.VariantName,
                VariantOrigin = variant.VariantOrigin,
                VariantDateDiscovered = variant.VariantDateDiscovered,
                CountriesIds = variant.Countries.Select(c => c.Id).ToList(),
                SymptomsIds = variant.Symptoms.Select(s => s.Id).ToList()
            };

            ViewBag.VariantName = variantEdit.VariantName;
            return View(variantEdit);
        }



        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VariantsEdit variantEdit)//[Bind("Id,CountryName")] Country country)
        {
            if (id != variantEdit.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var entity in _context.CountriesVariants.AsNoTracking())
                {
                    if (entity.VariantId == id)
                        _context.CountriesVariants.Remove(entity);
                }
                foreach (var entity in _context.SymptomsVariants.AsNoTracking())
                {
                    if (entity.VariantId == id)
                        _context.SymptomsVariants.Remove(entity);
                }
                _context.SaveChanges();
                var variant = await _context.Variants.AsNoTracking().Include(c => c.Countries).FirstOrDefaultAsync(d => d.Id == variantEdit.Id);
                if (variant is null)
                    return NotFound();
                var variantIndices = _context.Variants.AsNoTracking().Select(c => c.Countries).ToList();

                Virus vr = new Virus();
                foreach (var v in _context.Viruses)
                {
                    if (v.Id == variantEdit.VirusId)
                    {
                        variant.Virus = v;
                        break;
                    }
                }

                variant.VariantName = variantEdit.VariantName;
                variant.VariantDateDiscovered = variantEdit.VariantDateDiscovered;
                var countries = await _context.Countries.AsNoTracking().Where(v => variantEdit.CountriesIds.Contains(v.Id)).ToListAsync();
                variant.Countries = countries;
                var symptoms = await _context.Symptoms.AsNoTracking().Where(v => variantEdit.SymptomsIds.Contains(v.Id)).ToListAsync();
                variant.Symptoms = symptoms;
                //Variant variant = new Variant();
                variant.Id = variantEdit.Id;
                variant.VariantName = variantEdit.VariantName;
                variant.VariantOrigin = variantEdit.VariantOrigin;
                variant.Symptoms = variant.Symptoms;
                variant.Countries = variant.Countries;
                try
                {
                    _context.Update(variant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VariantExists(variant.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }






        // GET: Variants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variant = await _context.Variants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variant == null)
            {
                return NotFound();
            }

            return View(variant);
        }

        // POST: Variants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variant = await _context.Variants.FindAsync(id);
            _context.Variants.Remove(variant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariantExists(int id)
        {
            return _context.Variants.Any(e => e.Id == id);
        }
    }
}
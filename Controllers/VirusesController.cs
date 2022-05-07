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
    public class VirusesController : Controller
    {
        private readonly DBFinalContext _context;

        public VirusesController(DBFinalContext context)
        {
            _context = context;
        }

        // GET: Viruses
        public async Task<IActionResult> Index(int? id, string name)
        {
            if (id == null || name == null)
            {
                var viruses = _context.Viruses.Include(x => x.Group);
                return View(await viruses.ToListAsync());
            }
            ViewBag.GroupId = id;
            ViewBag.GroupName = name;
            var virusesByGroup = _context.Viruses.Where(x => x.GroupId == id).Include(x => x.Group);
            return View(await virusesByGroup.ToListAsync());
        }

        // GET: Virus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var virus = await _context.Viruses
                .Include(d => d.Group)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (virus == null)
                return NotFound();

            //return View(virus);
            return RedirectToAction("Index", "Variants", new { id = virus.Id, name = virus.VirusName });
        }


        // GET: Viruses/Create
        public IActionResult Create(int? groupId)
        {
            if (groupId == null)
            {
                ViewData["GroupId"] = new SelectList(_context.VirusGroups, "Id", "GroupName");
            }
            else
            {
                ViewBag.GroupId = groupId;
                ViewBag.GroupName = _context.VirusGroups.Where(f => f.Id == groupId).FirstOrDefault().GroupName;
            }
            return View();
        }

        // POST: Viruses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? groupId, [Bind("Id,VirusDateDiscovered, VirusName, GroupId")] Virus virus)
        {
            virus.GroupId = groupId;
            virus.Group = await _context.VirusGroups.FindAsync(virus.GroupId);
            ModelState.ClearValidationState(nameof(virus.Group));
            TryValidateModel(virus.Group, nameof(virus.Group));

            //if (groupId == null)
            //{
            //if (ModelState.IsValid)
            //    {
            //        _context.Add(virus);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    return View(virus);
            //}

            //virus.GroupId = groupId;
            //virus.Group = await _context.VirusGroups.FindAsync(virus.GroupId);
            //ModelState.ClearValidationState(nameof(virus.Group));
            //TryValidateModel(virus.Group, nameof(virus.Group));
            if (ModelState.IsValid)
            {
                _context.Add(virus);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Viruses", new { id = groupId, name = _context.VirusGroups.Where(c => c.Id == groupId).FirstOrDefault().GroupName });
            }
            return RedirectToAction("Index", "Viruses", new { id = groupId, name = _context.VirusGroups.Where(c => c.Id == groupId).FirstOrDefault().GroupName });
        }

        // GET: Viruses/Edit/5
        public async Task<IActionResult> Edit(int? id, int? groupId)
        {
            if (id == null)
                return NotFound();

            var virus = await _context.Viruses.FindAsync(id);
            virus.Group = await _context.VirusGroups.FindAsync(virus.GroupId);
            ViewBag.GroupId = virus.GroupId;
            if (groupId != null)
            {
                ViewBag.GroupName = _context.VirusGroups.Where(f => f.Id == virus.GroupId).FirstOrDefault().GroupName;
                ViewBag.GroupId = groupId;
            }
            else
            {
                ViewBag.GroupName = null;
                ViewData["GroupId"] = new SelectList(_context.VirusGroups, "Id", "GroupName", groupId);
            }

            if (virus == null)
                return NotFound();

            return View(virus);
        }

        // POST: Viruses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GroupId,VirusName,VirusDateDiscovered")] Virus virus)
        {
            if (id != virus.Id)
                return NotFound();

            virus.Group = await _context.VirusGroups.FindAsync(virus.GroupId);
            ModelState.ClearValidationState(nameof(virus.Group));
            TryValidateModel(virus.Group, nameof(virus.Group));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(virus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VirusExists(virus.Id))
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
            ViewData["GroupId"] = new SelectList(_context.VirusGroups, "Id", "GroupName", virus.GroupId);
            return View(virus);
            //return RedirectToAction("Index", "Viruses", new { id = virus.GroupId, name = _context.VirusGroups.Where(f => f.Id == virus.GroupId).FirstOrDefault().GroupName });
        }

        // GET: Viruses/Delete/5
        public async Task<IActionResult> Delete(int? id, int? groupId)
        {
            if (id == null)
                return NotFound();

            var virus = await _context.Viruses
                .Include(d => d.Group)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (groupId != null)
                ViewBag.GroupName = _context.VirusGroups.Where(f => f.Id == virus.GroupId).FirstOrDefault().GroupName;
            else
                ViewBag.GroupName = null;

            if (virus == null)
                return NotFound();

            return View(virus);
        }

        // POST: Viruses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var virus = await _context.Viruses.FindAsync(id);
            _context.Viruses.Remove(virus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VirusExists(int id)
        {
            return _context.Viruses.Any(e => e.Id == id);
        }
    }
}
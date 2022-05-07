#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabaOne;
using ClosedXML.Excel;

namespace LabaOne.Controllers
{
    public class VirusGroupsController : Controller
    {
        private readonly DBFinalContext _context;

        public VirusGroupsController(DBFinalContext context)
        {
            _context = context;
        }

        // GET: VirusGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.VirusGroups.ToListAsync());
        }

        // GET: viruses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var virus = await _context.VirusGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virus == null)
                return NotFound();
            return RedirectToAction("Index", "Viruses", new { id = virus.Id, name = virus.GroupName });
            //return RedirectToAction("Index", "VirusGroups", new { id = virus.Id, name = virus.GroupName });
        }

        // GET: viruses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: viruses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, GroupName, GroupInfo, DateDiscovered")] VirusGroup group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // GET: viruses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _context.VirusGroups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, GroupName, GroupInfo, DateDiscovered")] VirusGroup group)
        {
            if (id != group.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(group.Id))
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
            return View(group);
        }

        // GET: viruses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.VirusGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: viruses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _context.VirusGroups.FindAsync(id);
            var virus = _context.Viruses.Where(c => c.GroupId == id);
            _context.VirusGroups.Remove(group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return _context.VirusGroups.Any(e => e.Id == id);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Import(IFormFile fileExcel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (fileExcel != null)
        //        {
        //            using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
        //            {
        //                await fileExcel.CopyToAsync(stream);
        //                using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
        //                {
        //                    //перегляд усіх листів (в даному випадку категорій) 

        //                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
        //                    {
        //                        //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову 

        //                        VirusGroup newgroup;
        //                        var g = (from gr in _context.VirusGroups
        //                                 where gr.GroupName.Contains(worksheet.Name)
        //                                 select gr).ToList();
        //                        if (g.Count > 0) {
        //                            newgroup = g[0];
        //                        }
        //                        else {
        //                            newgroup = new VirusGroup();
        //                            newgroup.GroupName = worksheet.Name;
        //                            newgroup.GroupInfo = "from EXCEL";
        //                            //додати в контекст 
        //                            _context.VirusGroups.Add(newgroup);
        //                        }
        //                        //перегляд усіх рядків                     
        //                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1)) {
        //                            try {
        //                                Virus virus = new Virus();
        //                                virus.VirusName = row.Cell(1).Value.ToString();
        //                                //virus.Info = row.Cell(6).Value.ToString();
        //                                virus.Group = newgroup;
        //                                _context.Viruses.Add(virus);
        //                                //у разі наявності автора знайти його, у разі відсутності - додати 
        //                                for (int i = 2; i <= 5; i++) {
        //                                    if (row.Cell(i).Value.ToString().Length > 0) {
        //                                        Author author;
        //                                        var a = (from aut in _context.Authors

        //                                                 where aut.Name.Contains(row.Cell(i).Value.ToString())

        //                                                 select aut).ToList();

        //                                        if (a.Count > 0)

        //                                        {

        //                                            author = a[0];

        //                                        }

        //                                        else

        //                                        {

        //                                            author = new Author();

        //                                            author.Name = row.Cell(i).Value.ToString();

        //                                            author.Info = "from EXCEL";

        //                                            //додати в контекст 
        //                                            _context.Add(author);
        //                                        }
        //                                        AuthorBook ab = new AuthorBook();
        //                                        ab.Book = book;
        //                                        ab.Author = author;
        //                                        _context.AuthorBooks.Add(ab);
        //                                    }
        //                                }
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                //logging самостійно :) 

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction(nameof(Index));
        //}

    }

}

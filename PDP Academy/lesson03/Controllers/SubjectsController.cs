using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDP_Academy.DAL;
using PDP_Academy.Models;

namespace PDP_Academy.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly PdpDbContext _context;

        public SubjectsController(PdpDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSort"] = sortOrder == "title_asc" ? "title_desc" : "title_asc";
            ViewData["DescriptionSort"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewData["PriceSort"] = sortOrder == "price_asc" ? "price-desc" : "price_asc";
            ViewData["NumberOfModulesSort"] = sortOrder == "numberOfModules_asc" ? "numberOfModules_desc" : "numberOfModules_asc";
            ViewData["TotalHoursSort"] = sortOrder == "totalHours_asc" ? "totalHours_desc" : "totalHours_asc";

            var subjects = _context.Subjects.AsQueryable();

            subjects = sortOrder switch
            {
                "title_asc" => subjects.OrderBy(x => x.Title),
                "title_desc" => subjects.OrderByDescending(x => x.Title),
                "description_asc" => subjects.OrderBy(x => x.Description),
                "description_desc" => subjects.OrderByDescending(x => x.Description),
                "price_asc"=>subjects.OrderBy(x => x.Price),
                "price_desc"=>subjects.OrderByDescending(x=>x.Price),
                "numberOfModules_asc"=>subjects.OrderBy(x=>x.NumberOfModules),
                "numberOfModules-desc"=>subjects.OrderByDescending(x=>x.NumberOfModules),
                "totalHours_asc"=>subjects.OrderBy(x=>x.TotalHours),
                "totalHours_desc"=>subjects.OrderByDescending(x=>x.TotalHours),
                _ => subjects.OrderBy(x => x.Id)
            };
            return View(subjects);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string? searchString,string a)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(await _context.Subjects.ToListAsync());
            }

            var subjects = await _context.Subjects
                .Include(s => s.Assignments)
                .Include(s=>s.Courses)


                .Where(s => s.Title.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();
            return View(subjects);
        }
        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,NumberOfModules,TotalHours")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,NumberOfModules,TotalHours")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'PdpDbContext.Subjects'  is null.");
            }
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
          return (_context.Subjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

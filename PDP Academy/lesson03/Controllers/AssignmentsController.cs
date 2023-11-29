using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PDP_Academy.DAL;
using PDP_Academy.Models;

namespace PDP_Academy.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly PdpDbContext _context;

        public AssignmentsController(PdpDbContext context)
        {
            _context = context;
        }

        // GET: Assignments
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TeacherIdSort"] = sortOrder == "teacherid_asc" ? "teacherid_desc" : "teacherid_asc";
            ViewData["SubjectIdSort"] = sortOrder == "subjectid_asc" ? "subjectid_desc" : "subjectid_asc";

            var assignments = _context.Assignments.AsQueryable();

            assignments = sortOrder switch
            {
                "teacherid_asc" => assignments.OrderBy(x => x.Teacher),
                "teacherid_desc" => assignments.OrderByDescending(x => x.Teacher),
                "subjectid_asc" => assignments.OrderBy(x => x.Subject),
                "subjectid_desc" => assignments.OrderByDescending(x => x.Subject),
                _ => assignments.OrderBy(x => x.Id)
            };
            return View(assignments);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int SearchId)
        {
            var assignments = _context.Assignments.AsQueryable();
            if (!AssignmentExists(SearchId))
            {
                assignments = assignments
                    .Include(a => a.Teacher)
                    .Include(a => a.Subject);
                return View(await assignments.ToListAsync());
            }
            assignments = assignments
                .Where(a => a.SubjectId == SearchId)
                .Include(a => a.Teacher)
                .Include(a => a.Subject);
            return View(await assignments.ToListAsync());
        }

        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.Subject)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeacherId,SubjectId")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", assignment.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", assignment.TeacherId);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", assignment.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", assignment.TeacherId);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeacherId,SubjectId")] Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
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
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", assignment.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", assignment.TeacherId);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.Subject)
                .Include(a => a.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assignments == null)
            {
                return Problem("Entity set 'PdpDbContext.Assignments'  is null.");
            }
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
          return (_context.Assignments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

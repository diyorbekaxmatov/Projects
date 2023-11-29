using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PDP_Academy.DAL;
using PDP_Academy.Models;

namespace PDP_Academy.Controllers
{
    public class CourseGroupsController : Controller
    {
        private readonly PdpDbContext _context;

        public CourseGroupsController(PdpDbContext context)
        {
            _context = context;
        }

        // GET: CourseGroups
        public async Task<IActionResult> Index()
        {
            var pdpDbContext = _context.Groups.Include(c => c.Subject).Include(c => c.Teacher);
            return View(await pdpDbContext.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Index(string searchString)
        {
            var courseGroups = _context.Groups.AsQueryable();
            if (string.IsNullOrEmpty(searchString))
            {
                courseGroups = courseGroups
                    .Include(cg => cg.Subject)
                    .Include(cg => cg.Teacher);
                return View(await courseGroups.ToListAsync());

            }
            courseGroups = courseGroups
                .Where(cg => cg.Name.ToLower().Contains(searchString.ToLower()))
                .Include(cg => cg.Subject)
                .Include(cg => cg.Teacher);
            return View(await courseGroups.ToListAsync());

        }
        // GET: CourseGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var courseGroup = await _context.Groups
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseGroup == null)
            {
                return NotFound();
            }

            return View(courseGroup);
        }

        // GET: CourseGroups/Create
        public IActionResult Create()
        {
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
            return View();
        }

        // POST: CourseGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,ExpectedFinishDate,ActualFinishDate,SubjectId,TeacherId")] CourseGroup courseGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", courseGroup.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", courseGroup.TeacherId);
            return View(courseGroup);
        }

        // GET: CourseGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var courseGroup = await _context.Groups.FindAsync(id);
            if (courseGroup == null)
            {
                return NotFound();
            }
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", courseGroup.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", courseGroup.TeacherId);
            return View(courseGroup);
        }

        // POST: CourseGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,ExpectedFinishDate,ActualFinishDate,SubjectId,TeacherId")] CourseGroup courseGroup)
        {
            if (id != courseGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseGroupExists(courseGroup.Id))
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
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "Id", courseGroup.SubjectId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id", courseGroup.TeacherId);
            return View(courseGroup);
        }

        // GET: CourseGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var courseGroup = await _context.Groups
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseGroup == null)
            {
                return NotFound();
            }

            return View(courseGroup);
        }

        // POST: CourseGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'PdpDbContext.Groups'  is null.");
            }
            var courseGroup = await _context.Groups.FindAsync(id);
            if (courseGroup != null)
            {
                _context.Groups.Remove(courseGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseGroupExists(int id)
        {
          return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDP_Academy.DAL;
using PDP_Academy.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDP_Academy.ViewModels;

namespace PDP_Academy.Controllers
{
    public class StudentsController : Controller
    {
        private readonly PdpDbContext _context;

        public StudentsController(PdpDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FullNameSort"] = sortOrder == "fullName_asc" ? "fullName_desc" : "fullName_asc";
            ViewData["AgeSort"] = sortOrder == "age_asc" ? "age_desc" : "age_asc";

            var students = _context.Students.AsQueryable();

            students = sortOrder switch
            {
                "fullName_asc" => students.OrderBy(x => x.FullName),
                "fullName_desc" => students.OrderByDescending(x => x.FullName),
                "age_asc" => students.OrderBy(x => x.Age),
                "age_desc" => students.OrderByDescending(x => x.Age),
                _ => students.OrderBy(x => x.Id)
            };

            var groups = await _context.Groups.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToListAsync();   

            var studentsViewModel = new StudentViewModel()
            {
                Students = await students.ToListAsync(),
                Groups = groups
            };

            return View(studentsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? searchString, string group)
        {
            if (searchString == null && group == "All")
            {
                var students1 = _context.Students.ToList();

                var groups1 = await _context.Groups.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var studentVM1 = new StudentViewModel()
                {
                    Students = students1,
                    Groups = groups1
                };

                return View(studentVM1);
            }
            else if (group == "All")
            {
                var students1 = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Group)
                .Where(s => s.FullName.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();

                var groups1 = await _context.Groups.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var studentVM1 = new StudentViewModel()
                {
                    Students = students1,
                    Groups = groups1
                };
                return View(studentVM1);
            }
            else if (searchString == null)
            {
                int groupId1 = int.Parse(group);
                var students1 = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Group)
                .Where(x => x.Enrollments.Any(e => e.GroupId == groupId1))
                .ToListAsync();

                var groups1 = await _context.Groups.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var studentVM1 = new StudentViewModel()
                {
                    Students = students1,
                    Groups = groups1
                };
                return View(studentVM1);
            }
            else
            {
                int groupId = int.Parse(group);

                var students = await _context.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Group)
                    .Where(s => s.FullName.ToLower().Contains(searchString.ToLower()))
                    .Where(x => x.Enrollments.Any(e => e.GroupId == groupId))
                    .ToListAsync();

                var groups = await _context.Groups.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var studentVM = new StudentViewModel()
                {
                    Students = students,
                    Groups = groups
                };

                return View(studentVM);
            }
        }
        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Age")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'PdpDbContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

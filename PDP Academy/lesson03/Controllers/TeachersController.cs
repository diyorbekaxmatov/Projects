using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PDP_Academy.DAL;
using PDP_Academy.Models;

namespace PDP_Academy.Controllers
{
    public class TeachersController : Controller
    {
        private readonly PdpDbContext _context;

        public TeachersController(PdpDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstNameSort"] = sortOrder == "firstname_asc" ? "firstname_desc" : "firstname_asc";
            ViewData["LastNameSort"] = sortOrder == "lastname_asc" ? "lastname_desc" : "lastname_asc";
            ViewData["PhoneNumberSort"] = sortOrder == "phonenumber_asc" ? "phonenumber-desc" : "phonenumber_asc";
            ViewData["HourlyRateSort"] = sortOrder == "hourlyrate_asc" ? "hourlyrate_desc" : "hourlyrate_asc";

            var teachers = _context.Teachers.AsQueryable();

            teachers = sortOrder switch
            {
                "firstname_asc" => teachers.OrderBy(x => x.FirstName),
                "firstname_desc" => teachers.OrderByDescending(x => x.FirstName),
                "lastname_asc" => teachers.OrderBy(x => x.LastName),
                "lastname_desc" => teachers.OrderByDescending(x => x.LastName),
                "phonenumber_asc" => teachers.OrderBy(x => x.PhoneNumber),
                "phonenumber_desc" => teachers.OrderByDescending(x => x.PhoneNumber),
                "hourlyrate_asc" => teachers.OrderBy(x => x.HourlyRate),
                "hourlyrate-desc" => teachers.OrderByDescending(x => x.HourlyRate),

                _ => teachers.OrderBy(x => x.Id)
            }; ;
            return View(teachers);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? searchString,string a)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(await _context.Teachers.ToListAsync());
            }

            var teachers = await _context.Teachers
                .Include(s => s.Assignments)
                .Include(s => s.Courses)
                .Where(s => s.FirstName.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();

            return View(teachers);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,HourlyRate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,HourlyRate")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'PdpDbContext.Teachers'  is null.");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
          return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EXAM.Data;
using EXAM.Models;
using Microsoft.Data.SqlClient;

namespace EXAM.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly BookShopDbContext _context;

        public AuthorsController(BookShopDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index(string? searchString, string a)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return View(await _context.Authors.ToListAsync());
            }

            var authors = await _context.Authors
                .Where(s => s.FullName.ToLower().Contains(searchString.ToLower()))
                .ToListAsync();

            return View(authors);
        }

        // GET: Authors
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSort"] = sortOrder == "Id_asc" ? "Id_desc" : "Id_asc";
            ViewData["FullNameSort"] = sortOrder == "FullName_asc" ? "FullName_desc" : "FullName_asc";
            ViewData["BirhtdaySort"] = sortOrder == "Birhtday_asc" ? "Birhtday_asc" : "Birhtday_asc";

            var author = _context.Authors.AsQueryable();

            author = sortOrder switch
            {
                "Id_asc"=> author.OrderBy(x=>x.Id),
                "Id_desc"=>author.OrderByDescending(x=>x.Id),
                "FullName_asc" => author.OrderBy(x => x.FullName),
                "FullName_desc" => author.OrderByDescending(x => x.FullName),
                "Birhtday_asc" => author.OrderBy(x => x.Brithday),
                "Birhtday_desc" => author.OrderByDescending(x => x.Brithday),
                _ => author.OrderBy(x => x.Id)
            }; ;
            return View(author);
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Brithday")] Author author)
        {
            if (author!=null)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Brithday")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (author!=null)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookShopDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

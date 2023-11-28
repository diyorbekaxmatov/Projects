using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EXAM.Data;
using EXAM.Models;
using EXAM.ViewModels;


namespace EXAM.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookShopDbContext _context;

        public BooksController(BookShopDbContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["IdSort"] = sortOrder == "Id_asc" ? "Id_desc" : "Id_asc";
            ViewData["NameSort"] = sortOrder == "Name_asc" ? "Name_desc" : "Name_asc";
            ViewData["DescriptionSort"] = sortOrder == "Description_asc" ? "Description_desc" : "Description_asc";
            ViewData["CategoryIdSort"] = sortOrder == "CategoryId_asc" ? "CategoryId_desc" : "CategoryId_asc";
            ViewData["PriceSort"] = sortOrder == "Price_asc" ? "Price_desc" : "Price_asc";
            ViewData["AuthorIdSort"] = sortOrder == "AuthorId_asc" ? "AuthorId_desc" : "AuthorId_asc";

            var books = _context.Books.AsQueryable();

            books = sortOrder switch
            {
                "Id_asc" => books.OrderBy(x => x.Id),
                "Id_desc" => books.OrderByDescending(x => x.Id),
                "Name_asc" => books.OrderBy(x => x.Name),
                "Name_desc" => books.OrderByDescending(x => x.Name),
                "Description_asc" => books.OrderBy(x => x.Description),
                "Description_desc" => books.OrderByDescending(x => x.Description),
                "CategoryId_asc" => books.OrderBy(x => x.CategoryId),
                "CategoryId_desc" => books.OrderByDescending(x => x.CategoryId),
                "Price_asc" => books.OrderBy(x => x.Price),
                "Price_desc" => books.OrderByDescending(x => x.Price),
                "AuthorId_asc" => books.OrderBy(x => x.AuthorId),
                "AuthorId_desc" => books.OrderByDescending(x => x.AuthorId),
                _ => books.OrderBy(x => x.Id)
            };
            var categories = await _context.Categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToListAsync();

            var bookViewModel = new BookViewModel()
            {
                Books = await books.ToListAsync(),
                Categories = categories
            };

            return View(bookViewModel);

        }
            
        [HttpPost]
        public async Task<IActionResult> Index(string? searchString, string category)
        {   
            if (searchString != null && category.Count() == 1)
            {
                int categoryId = int.Parse(category);

                var books = await _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.Name.ToLower().Contains(searchString.ToLower()))
                    .Where(b => b.CategoryId == categoryId)
                    .ToListAsync();

                var categories = await _context.Categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var bookVM = new BookViewModel()
                {
                    Books = books,
                    Categories = categories
                };

                return View(bookVM);
            }
            else if (searchString == null && category.Count() == 1)
            {
                int categoryId = int.Parse(category);

                var books = await _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.CategoryId == categoryId)
                    .ToListAsync();

                var categories = await _context.Categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var bookVM = new BookViewModel()
                {
                    Books = books,
                    Categories = categories
                };

                return View(bookVM);
            }
            else if (searchString != null && category.Count() > 1)
            {
                var books = await _context.Books
                    .Include(b => b.Category)
                    .Where(b => b.Name.ToLower().Contains(searchString.ToLower()))
                    .ToListAsync();

                var categories = await _context.Categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var bookVM = new BookViewModel()
                {
                     Books = books,
                     Categories = categories
                };

                return View(bookVM);
            }
            else
            {
                var books = await _context.Books
                    .Include(b => b.Category)
                    .ToListAsync();

                var categories = await _context.Categories.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToListAsync();

                var bookVM = new BookViewModel()
                {
                    Books = books,
                    Categories = categories
                };
                return View(bookVM);
            }
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CategoryId,Price,AuthorId")] Book book)
        {
            if (book!=null)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FullName", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CategoryId,Price,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (book!=null)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'BookShopDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

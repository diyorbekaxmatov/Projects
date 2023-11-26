using EXAM.Models;
using Microsoft.EntityFrameworkCore;

namespace EXAM.Data
{
    public class BookShopDbContext:DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }

        public BookShopDbContext(DbContextOptions<BookShopDbContext> options) :
            base(options)
        {
            Database.Migrate();
        }
    }
}

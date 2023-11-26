using Bogus;
using Bogus.DataSets;
using EXAM.Data;
using EXAM.Models;
using Microsoft.EntityFrameworkCore;

namespace EXAM.Extensions
{
    public static class DatabaseSeeder
    {
        private static Faker _faker = new Faker();

        public static void SeedDatabase(this IServiceCollection _, IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<BookShopDbContext>>();
            using var context = new BookShopDbContext(options);

            CreateCategories(context);
            CreateAuthors(context);
            CreateBooks(context);
        }
        private static void CreateCategories(BookShopDbContext context)
        {
            if (context.Categories.Any()) return;
            List<Category> categories = new();

            for (int i = 0; i < 10; i++)
            {
                categories.Add(new Category()
                {
                    Name = _faker.Name.FullName()
                }) ;
            }
            context.AddRange(categories);
            context.SaveChanges();
        }
        private static void CreateAuthors(BookShopDbContext context)
        {

            if(context.Authors.Any()) return;
            List<Author> authors = new();
            for(int i = 0;i < 10; i++)
            {
                authors.Add(new Author()
                {
                    FullName = _faker.Name.FullName(),
                    Brithday = _faker.Date.Between(DateTime.Now.AddYears(-200), DateTime.Now.AddYears(-50))
                });
            }
            context.AddRange(authors);
            context.SaveChanges();
        }
        private static void CreateBooks(BookShopDbContext context)
        {
            if( context.Books.Any()) return;
            List<Book> books = new();
            var authors = context.Authors.ToList();
            var categorys= context.Categories.ToList();

            for(int i = 0; i < 10; i++)
            {
                var randomAuthor = _faker.PickRandom(authors);
                var randomCategorys = _faker.PickRandom(categorys);
                books.Add(new Book()
                {
                    Name = _faker.Commerce.ProductName(),
                    CategoryId = randomCategorys.Id,
                    Description=_faker.Commerce.ProductDescription(),
                    Price=_faker.Random.Decimal(10000,1000000),
                    AuthorId= randomAuthor.Id,
                });
            }
            context.AddRange(books);
            context.SaveChanges();
            
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace EXAM.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId {  get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        ICollection<Author> Authors { get; set; }
    }
}

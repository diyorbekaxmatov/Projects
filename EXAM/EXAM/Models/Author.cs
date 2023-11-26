using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EXAM.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Brithday { get; set; } 
        public ICollection<Book> Books { get; set; }    
    }
}

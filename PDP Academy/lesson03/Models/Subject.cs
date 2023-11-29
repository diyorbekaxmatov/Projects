using System.Reflection;

namespace PDP_Academy.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int NumberOfModules { get; set; }
        public int TotalHours { get; set; }

        public virtual ICollection<CourseGroup> Courses { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Module> Modules { get; set; }

        public Subject()
        {
            Courses = new List<CourseGroup>();
            Assignments = new List<Assignment>();
        }
    }
}

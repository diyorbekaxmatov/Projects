namespace PDP_Academy.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal HourlyRate { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<CourseGroup> Courses { get; set; }

        public Teacher()
        {
            Assignments = new List<Assignment>();
            Courses = new List<CourseGroup>();
        }
    }
}

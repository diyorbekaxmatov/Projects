using PDP_Academy.Models.Enums;
namespace PDP_Academy.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public StudentStatus Status { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int GroupId { get; set; }
        public CourseGroup Group { get; set; }
    }
}

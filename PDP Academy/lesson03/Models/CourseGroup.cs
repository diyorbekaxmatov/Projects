namespace PDP_Academy.Models
{
    public class CourseGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpectedFinishDate { get; set; }
        public DateTime? ActualFinishDate { get; set; }
        public int CurrentModule { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public CourseGroup()
        {
            Enrollments = new List<Enrollment>();
        }
    }
}

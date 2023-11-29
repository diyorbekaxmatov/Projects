namespace PDP_Academy.Models
{
    public class Module
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public virtual ICollection<ModuleTopic> Topics { get; set; }
    }
}

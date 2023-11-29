using PDP_Academy.Models.Enums;
using Microsoft.EntityFrameworkCore;
using PDP_Academy.Models;


namespace PDP_Academy.DAL
{
    public class PdpDbContext : DbContext
    {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<CourseGroup> Groups { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleTopic> ModuleTopics { get; set; }



        public PdpDbContext(DbContextOptions<PdpDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Student

            modelBuilder.Entity<Student>()
                .ToTable(nameof(Student));
            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<Student>()
                .Property(s => s.FullName)
                .HasMaxLength(255);
            modelBuilder.Entity<Student>()
                .Property(x => x.Balance)
                .HasColumnType("money");

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Enrollments)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .HasConstraintName("Student_Enrollment_FK");
            #endregion

            #region Teacher

            modelBuilder.Entity<Teacher>()
                .ToTable(nameof(Teacher));
            modelBuilder.Entity<Teacher>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Teacher>()
                .Property(t => t.FirstName)
                .HasMaxLength(255);
            modelBuilder.Entity<Teacher>()
                .Property(t => t.LastName)
                .HasMaxLength(255);
            modelBuilder.Entity<Teacher>()
                .Property(t => t.PhoneNumber)
                .HasColumnName("Phone");
            modelBuilder.Entity<Teacher>()
                .Property(t => t.HourlyRate)
                .HasColumnType("money");
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Assignments)
                .WithOne(a => a.Teacher)
                .HasForeignKey(a => a.TeacherId)
                .HasConstraintName("Teacher_Assignment_FK");
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Courses)
                .WithOne(cg => cg.Teacher)
                .HasForeignKey(cg => cg.TeacherId)
                .HasConstraintName("Teacher_Course_FK");

            #endregion

            #region Subject
            modelBuilder.Entity<Subject>()
                .ToTable(nameof(Subject));
            modelBuilder.Entity<Subject>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Subject>()
                .Property(s => s.Price)
                .HasColumnType("money");
            modelBuilder.Entity<Subject>()
                .Property(s => s.Title)
                .HasMaxLength(255);
            modelBuilder.Entity<Subject>()
                .Property(s => s.Description)
                .HasMaxLength(500);
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Courses)
                .WithOne(e => e.Subject)
                .HasForeignKey(e => e.SubjectId)
                .HasConstraintName("Subject_Course_FK");
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Assignments)
                .WithOne(a => a.Subject)
                .HasForeignKey(a => a.SubjectId)
                .HasConstraintName("Subject_Assignment_FK");
            #endregion

            #region Assignment

            modelBuilder.Entity<Assignment>()
                .ToTable(nameof(Assignment));
            modelBuilder.Entity<Assignment>()
                .HasKey(x => x.Id);

            #endregion

            #region Enrollment

            modelBuilder.Entity<Enrollment>()
                .ToTable(nameof(Enrollment));
            modelBuilder.Entity<Enrollment>()
                .HasAlternateKey(e => new
                {
                    e.GroupId,
                    e.StudentId
                });

            modelBuilder.Entity<Enrollment>()
                .Property(x => x.Status)
                .HasDefaultValue(StudentStatus.Active);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Enrollments)
                .HasForeignKey(e => e.GroupId)
                .HasConstraintName("Enrollment_Course_FK");

            #endregion

            #region Course Group

            modelBuilder.Entity<CourseGroup>()
                .ToTable("Course_Group");
            modelBuilder.Entity<CourseGroup>()
                .HasKey(cg => cg.Id);
            modelBuilder.Entity<CourseGroup>()
                .Property(cg => cg.Name)
                .HasMaxLength(255);
            modelBuilder.Entity<CourseGroup>()
                .Property(cg => cg.StartDate)
                .HasColumnType("date");
            modelBuilder.Entity<CourseGroup>()
                .Property(cg => cg.ExpectedFinishDate)
                .HasColumnType("date")
                .IsRequired(false);
            modelBuilder.Entity<CourseGroup>()
                .Property(cg => cg.ActualFinishDate)
                .HasColumnType("date")
                .IsRequired(false);
            modelBuilder.Entity<CourseGroup>()
                .Property(cg => cg.CurrentModule)
                .HasDefaultValue(1);

            #endregion

            #region Module

            modelBuilder.Entity<Module>()
                .ToTable(nameof(Module));
            modelBuilder.Entity<Module>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<Module>()
                .Property(x => x.Number);
            modelBuilder.Entity<Module>()
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired();
            modelBuilder.Entity<Module>()
                .Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<Module>()
                .HasOne(m => m.Subject)
                .WithMany(m => m.Modules)
                .HasForeignKey(m => m.SubjectId)
                .HasConstraintName("FK_Subject_Module");

            #endregion

            #region Module Topic

            modelBuilder.Entity<ModuleTopic>()
                .ToTable("Module_Topic");
            modelBuilder.Entity<ModuleTopic>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ModuleTopic>()
                .Property(x => x.Title)
                .HasMaxLength(255);
            modelBuilder.Entity<ModuleTopic>()
                .Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            modelBuilder.Entity<ModuleTopic>()
                .HasOne(mt => mt.Module)
                .WithMany(m => m.Topics)
                .HasForeignKey(mt => mt.ModuleId)
                .HasConstraintName("FK_Module_Topic");

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}

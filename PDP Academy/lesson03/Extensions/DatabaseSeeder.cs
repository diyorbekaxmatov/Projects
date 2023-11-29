using Bogus;
using PDP_Academy.DAL;
using PDP_Academy.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace PDP_Academy.Extensions
{
    public class DatabaseSeeder
    {
        static Faker _faker = new Faker();
        public static void Initialize(IServiceProvider provider)
        {
            using var context = new PdpDbContext(provider.GetRequiredService<DbContextOptions<PdpDbContext>>());

            try
            {
                CreateStudents(context);
                CreateTeachers(context);
                CreateSubjects(context);
                CreateAssignments(context);
                CreateGroups(context);
                CreateEnrollments(context);
                CreateModules(context);
                CreateModuleTopics(context);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        static void CreateStudents(PdpDbContext context)
        {
            if (context.Students.Any()) return;

            for (int i = 0; i < 500; i++)
            {
                var student = new Student()
                {
                    FullName = _faker.Name.FullName(),
                    Age = _faker.Random.Int(15, 50)
                };

                context.Students.Add(student);
            }

            context.SaveChanges();
        }

        static void CreateTeachers(PdpDbContext context)
        {
            if (context.Teachers.Any()) return;

            for (int i = 0; i < 15; i++)
            {
                var teacher = new Teacher()
                {
                    FirstName = _faker.Name.FirstName(),
                    LastName = _faker.Name.LastName(),
                    HourlyRate = _faker.Random.Decimal(300_000, 700_000),
                    PhoneNumber = _faker.Phone.PhoneNumber("+998 ## ###-##-##")
                };

                context.Teachers.Add(teacher);
            }

            context.SaveChanges();
        }

        static void CreateSubjects(PdpDbContext context)
        {
            if (context.Subjects.Any()) return;

            for (int i = 0; i < 50; i++)
            {
                var subject = new Subject()
                {
                    Title = _faker.Lorem.Word(),
                    Description = _faker.Lorem.Sentences(),
                    NumberOfModules = _faker.Random.Int(5, 14),
                    Price = _faker.Random.Decimal(1_000_000, 3_000_000),
                    TotalHours = _faker.Random.Int(30, 50)
                };

                context.Subjects.Add(subject);
            }

            context.SaveChanges();
        }

        static void CreateAssignments(PdpDbContext context)
        {
            if (context.Assignments.Any()) return;

            var teachers = context.Teachers.ToList();
            var subjects = context.Subjects.ToList();
            List<Assignment> assignments = new List<Assignment>();

            if (!teachers.Any() || !subjects.Any()) return;

            foreach (var teacher in teachers)
            {
                var teacherAssignmentCount = new Random().Next(1, 5);

                for (int i = 0; i < teacherAssignmentCount; i++)
                {
                    var randomSubjectIndex = new Random().Next(0, subjects.Count - 1);
                    var subject = subjects[randomSubjectIndex];

                    var assignment = new Assignment()
                    {
                        Teacher = teacher,
                        Subject = subject
                    };

                    if (assignments.Any(x => x.Teacher == teacher && x.Subject == subject))
                    {
                        continue;
                    }

                    assignments.Add(assignment);
                }
            }

            context.Assignments.AddRange(assignments);
            context.SaveChanges();
        }

        static void CreateGroups(PdpDbContext context)
        {
            if (context.Groups.Any()) return;

            var assignmnents = context.Assignments.ToList();
            List<CourseGroup> groups = new List<CourseGroup>();

            foreach (var assignment in assignmnents)
            {
                var numberOfCourses = new Random().Next(1, 6);

                for (int i = 0; i < numberOfCourses; i++)
                {
                    var startDate = DateTime.Now.AddMonths(new Random().Next(-8, -1));
                    var finishDate = DateTime.Now.AddMonths(new Random().Next(1, 8));
                    var actualFinishDate = DateTime.Now.AddMonths(new Random().Next(1, 9));

                    var courseGroup = new CourseGroup()
                    {
                        Name = _faker.Lorem.Word(),
                        StartDate = startDate,
                        ExpectedFinishDate = finishDate,
                        ActualFinishDate=actualFinishDate,
                        Subject = assignment.Subject,
                        Teacher = assignment.Teacher,
                    };

                    // Teacher can only have 6 groups at the same time
                    if (groups.Count(cg => cg.TeacherId == assignment.TeacherId) > 6)
                    {
                        continue;
                    }

                    groups.Add(courseGroup);
                }
            }

            context.Groups.AddRange(groups);
            context.SaveChanges();
        }

        static void CreateEnrollments(PdpDbContext context)
        {
            if (context.Enrollments.Any()) return;

            var students = context.Students.ToList();
            var groups = context.Groups.ToList();
            List<Enrollment> enrollments = new List<Enrollment>();

            if (!students.Any() || !groups.Any()) return;

            foreach (var student in students)
            {
                var numberOfGroups = new Random().Next(1, 4);

                for (int i = 0; i < numberOfGroups; i++)
                {
                    var groupIndex = new Random().Next(0, groups.Count - 1);
                    var group = groups[groupIndex];

                    var enrollment = new Enrollment()
                    {
                        Student = student,
                        Group = group
                    };

                    if (enrollments.Any(e => e.Student == student && e.Group == group))
                    {
                        continue;
                    }

                    enrollments.Add(enrollment);
                }
            }

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
        static void CreateModules(PdpDbContext context)
        {
            if (context.Modules.Any()) return;

            var subjects = context.Subjects.ToList();

            foreach (var subject in subjects)
            {
                for (int i = 1; i <= 10; i++)
                {
                    context.Modules.Add(new Module()
                    {
                        SubjectId = subject.Id,
                        Name = $"Module-{i}",
                        Description = _faker.Lorem.Sentences(3),
                        Number = i,
                    });
                }
            }

            context.SaveChanges();
        }

        static void CreateModuleTopics(PdpDbContext context)
        {
            if (context.ModuleTopics.Any()) return;

            var modules = context.Modules.ToList();

            foreach (var module in modules)
            {
                for (int i = 1; i <= 12; i++)
                {
                    context.ModuleTopics.Add(new ModuleTopic()
                    {
                        ModuleId = module.Id,
                        Title = _faker.Lorem.Sentence(),
                        Description = _faker.Lorem.Sentences(4),
                    });
                }
            }

            context.SaveChanges();
        }
    }
}


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class AppDatabaseContext : IdentityDbContext<User>
    {
        #region Private properties
        private DbSet<Student> _students;
        private DbSet<Professor> _professors;
        private DbSet<Discipline> _disciplines;
        private DbSet<Homework> _homeworks;
        private DbSet<Project> _projects;
        private DbSet<StudentDiscipline> _studentDisciplines;
        #endregion

        #region Getters & Setters
        public DbSet<Student> Students
        {
            get => _students;
            set => _students = value;
        }
        public DbSet<Professor> Professors
        {
            get => _professors;
            set => _professors = value;
        }
        public DbSet<Discipline> Disciplines
        {
            get => _disciplines;
            set => _disciplines = value;
        }
        public DbSet<Homework> Homeworks
        {
            get => _homeworks;
            set => _homeworks = value;
        }

        public DbSet<Project> Projects
        {
            get => _projects;
            set => _projects = value;
        }

        public DbSet<StudentDiscipline> StudentDisciplines
        {
            get => _studentDisciplines;
            set => _studentDisciplines = value;
        }
        #endregion

        #region Constructors
        public AppDatabaseContext(DbContextOptions options) : base(options)
        {
        }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            base.OnModelCreating(builder);
            // Create a separate table for students and professors.
            // The Student and Professor tables will still have aspnetusers
            // properties but I didnt want to mix them all together because
            // the students and professors have different properties.
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Professor>().ToTable("Professors");

            // Configure the discriminator for the Homework hierarchy.
            // Dont remove these, otherwise the application wont work.
            builder.Entity<Homework>()
                .HasDiscriminator<string>("HomeworkType")
                .HasValue<Homework>("Homework")
                .HasValue<Project>("Project");

            //The necessary methods for creating the many to many relationship between Student and Discipline and specifying the foreign keys
            builder.Entity<StudentDiscipline>().HasKey(sd => new { sd.StudentId, sd.DisciplineId });
            builder.Entity<StudentDiscipline>().HasOne(sd => sd.Student).WithMany(s => s.StudentDisciplines).HasForeignKey(sd => sd.StudentId);
            builder.Entity<StudentDiscipline>().HasOne(sd => sd.Discipline).WithMany(d => d.StudentDisciplines).HasForeignKey(sd => sd.DisciplineId);

            // One to many relationship between discipline and homeworks.
            builder.Entity<Discipline>().
                HasMany(h => h.Homeworks).
                WithOne(d => d.Discipline).
                HasForeignKey(f => f.DisciplineId);

            // One to many relationship between student and homeworks.
            builder.Entity<Student>().
                HasMany(h => h.Homeworks).
                WithOne(s => s.Student).
                HasForeignKey(f => f.StudentId);

            // One to many relationship between discipline and professors.
            builder.Entity<Discipline>()
                .HasMany(d => d.Professors)
                .WithOne(p => p.Discipline)
                .HasForeignKey(p => p.DisciplineId);

            // One to many relationship between homework project and project files.
            builder.Entity<Project>()
                .HasMany(p => p.ProjectFiles)
                .WithOne(h => h.Homework)
                .HasForeignKey(k => k.HomeworkID);
                

            // Here add all the disciplines that exist.
            builder.Entity<Discipline>().HasData(
                new Discipline { Id = "1", Name = "Mathematics" },
                new Discipline { Id = "2", Name = "Physics"},
                new Discipline { Id = "3", Name = "ComputerScience"}
                );
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Identity;
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
        private DbSet<Admin> _admins;
        private DbSet<Discipline> _disciplines;
        private DbSet<Homework> _homeworks;
        private DbSet<Project> _projects;
        private DbSet<ProjectFile> _projectsFile;
        private DbSet<StudentDiscipline> _studentDisciplines;
        private DbSet<Quiz> _quizzes;
        private DbSet<QuizQuestion> _quizQuestions;
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

        public DbSet<Admin> Admins
        {
            get =>_admins;
            set => _admins = value;
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

        public DbSet<ProjectFile> ProjectFiles
        {
            get => _projectsFile;
            set => _projectsFile = value;
        }

        public DbSet<StudentDiscipline> StudentDisciplines
        {
            get => _studentDisciplines;
            set => _studentDisciplines = value;
        }

        public DbSet<Quiz> Quizzes
        {
            get => _quizzes;
            set => _quizzes = value;
        }

        public DbSet<QuizQuestion> QuizQuestions
        {
            get => _quizQuestions;
            set => _quizQuestions = value;
        }
        #endregion

        #region Constructors
        public AppDatabaseContext(DbContextOptions options) : base(options)
        {
        }
        #endregion

        #region Methods
        protected override async void OnModelCreating(ModelBuilder builder)
        {
           
            base.OnModelCreating(builder);
            // Create a separate table for students and professors.
            // The Student and Professor tables will still have aspnetusers
            // properties but I didnt want to mix them all together because
            // the students and professors have different properties.
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Professor>().ToTable("Professors");
            builder.Entity<Admin>().ToTable("Admins");

            // Configure the discriminator for the Homework hierarchy.
            // Dont remove these, otherwise the application wont work.
            builder.Entity<Homework>()
                .HasDiscriminator<string>("HomeworkType")
                .HasValue<Homework>("Homework")
                .HasValue<Project>("Project")
                .HasValue<Quiz>("Quiz");

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
                .WithOne(h => h.Project)
                .HasForeignKey(k => k.ProjectID);

            // One to many relationship between quizess and quiz questions.
            builder.Entity<Quiz>()
                .HasMany(qq => qq.QuizQuestions)
                .WithOne(q => q.Quiz)
                .HasForeignKey(k => k.QuizID);
                

            // Here add all the disciplines that exist.
            builder.Entity<Discipline>().HasData(
                new Discipline { Id = "1", Name = "Matematica" },
                new Discipline { Id = "2", Name = "Fizica"},
                new Discipline { Id = "3", Name = "Programare"}
                );
        }
        #endregion
    }
}

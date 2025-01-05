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
            //The necessary methods for creating the many to many relationship between Student and Discipline and specifying the foreign keys
            builder.Entity<StudentDiscipline>().HasKey(sd => new { sd.StudentId, sd.DisciplineId });
            builder.Entity<StudentDiscipline>().HasOne(sd => sd.Student).WithMany(s => s.StudentDisciplines).HasForeignKey(sd => sd.StudentId);
            builder.Entity<StudentDiscipline>().HasOne(sd => sd.Discipline).WithMany(d => d.StudentDisciplines).HasForeignKey(sd => sd.DisciplineId);

            #endregion
        }
    }
}

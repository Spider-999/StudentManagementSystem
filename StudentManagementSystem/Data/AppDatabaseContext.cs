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
            // The Students and Professors tables will still have aspnetusers
            // properties but I didnt want to mix them all together because
            // the students and professors have different properties.
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Professor>().ToTable("Professors");
        }
        #endregion
    }
}

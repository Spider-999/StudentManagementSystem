using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Data
{
    public class AppDatabaseContext : IdentityDbContext<User>
    {
        #region Constructors
        public AppDatabaseContext(DbContextOptions options) : base(options)
        {
        }
        #endregion
    }
}

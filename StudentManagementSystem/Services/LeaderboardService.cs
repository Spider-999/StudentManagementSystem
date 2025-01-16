using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.ViewModels;

namespace StudentManagementSystem.Services
{
    /// <summary>
    /// Separated the leaderboard in a service class
    /// because both the student and the professor
    /// can view the leaderboard, thus I dont repeat
    /// the code.
    /// </summary>
    public class LeaderboardService
    {
        private readonly AppDatabaseContext _context;

        public LeaderboardService(AppDatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<LeaderboardViewModel>> GetLeaderboardAsync()
        {
            // Get all of the completed students homeworks by studentID
            var leaderboardData = await _context.Homeworks
                .Where(s => s.Status == true) 
                .GroupBy(si => si.StudentId)
                .Select(lvm => new LeaderboardViewModel
                {
                    StudentName = lvm.First().Student.Name,
                    Score = lvm.Count()
                })
                .OrderByDescending(s => s.Score) // Students are ordered by score
                .ToListAsync();

            return leaderboardData;
        }
    }
}

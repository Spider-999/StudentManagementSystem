using StudentManagementSystem.Models;

namespace StudentManagementSystem.ViewModels
{
    public class UserAndRolesViewModel
    {
        #region Private proeprties
        private User _user;
        private IList<string> _roles;
        #endregion

        #region Getters & Setters
        public User User
        {
            get => _user;
            set => _user = value;
        }

        public IList<string> Roles
        {
            get => _roles;
            set => _roles = value;
        }
        #endregion
    }
}

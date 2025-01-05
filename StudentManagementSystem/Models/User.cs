using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Models
{
    public class User : IdentityUser
    {
        #region Private properties
        private string _Name;
        #endregion

        #region Setters & Getters
        
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        #endregion
    }
}

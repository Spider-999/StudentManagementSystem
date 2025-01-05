using Microsoft.AspNetCore.Identity;

namespace StudentManagementSystem.Models
{
    public class User : IdentityUser
    {
        #region Private properties
        private string _name;
        #endregion

        #region Setters & Getters
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    /// <summary>
    /// Class that connects data to the register view.
    /// </summary>
    public class RegisterViewModel
    {
        #region Private properties
        private string _name;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private string _role;
        #endregion

        #region Setters & Getters

        [Required(ErrorMessage = "Numele este necesar.")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Required(ErrorMessage = "Email-ul este necesar.")]
        [EmailAddress(ErrorMessage = "Email-ul nu este valid.")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [Required(ErrorMessage = "Parola este necesara.")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [Required(ErrorMessage = "Confirmarea parolei este necesara.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola si confirmarea parolei nu se potrivesc.")]
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; }
        }

        [Required(ErrorMessage = "Rolul este necesar.")]
        public string Role
        {
            get { return _role; }
            set { _role = value; }

        }
        #endregion
    }
}

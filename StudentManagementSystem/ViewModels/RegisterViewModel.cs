using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    /// <summary>
    /// Class that connects data to the register view.
    /// </summary>
    public class RegisterViewModel
    {
        #region Private properties
        private string _Name;
        private string _Email;
        private string _Password;
        private string _ConfirmPassword;
        #endregion

        #region Setters & Getters

        [Required(ErrorMessage = "Numele este necesar.")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        [Required(ErrorMessage = "Email-ul este necesar.")]
        [EmailAddress(ErrorMessage = "Email-ul nu este valid.")]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        [Required(ErrorMessage = "Parola este necesara.")]
        [DataType(DataType.Password)]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        [Required(ErrorMessage = "Confirmarea parolei este necesara.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola si confirmarea parolei nu se potrivesc.")]
        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { _ConfirmPassword = value; }
        }
        #endregion
    }
}

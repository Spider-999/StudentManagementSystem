using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class CreateUserViewModel
    {
        #region Private properties
        private string _name;
        private string _email;
        private string _password;
        private string _confirmPassword;
        #endregion

        #region Getters & Setters

        [Required(ErrorMessage = "Numele este necesar.")]
        [Display(Name="Nume")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        [Required(ErrorMessage = "Email-ul este necesar.")]
        [EmailAddress(ErrorMessage = "Email-ul nu este valid.")]
        public string Email
        {
            get => _email;
            set => _email = value;
        }
        [Required(ErrorMessage = "Parola este necesara.")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password
        {
            get => _password;
            set => _password = value;
        }
        [Required(ErrorMessage = "Confirmarea parolei este necesara.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Parola si confirmarea parolei nu se potrivesc")]
        [Display(Name = "Confirmarea parolei")]
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = value;
        }
        #endregion
    }
}

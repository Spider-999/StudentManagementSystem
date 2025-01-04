using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        #region Private properties
        private string _email;
        private string _password;
        #endregion

        #region Getters & Setters
        [Required(ErrorMessage = "Email-ul este necesar")]
        [EmailAddress(ErrorMessage = "Adresa de email este invalida")]
        public string Email
        {
            get => _email;
            set => _email = value;
        }

        [Required(ErrorMessage = "Parola este necesara")]
        [DataType(DataType.Password)]
        public string Password
        {
            get => _password;
            set => _password = value;
        }
        #endregion
    }
}

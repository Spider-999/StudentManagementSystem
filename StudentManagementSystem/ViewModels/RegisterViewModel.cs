using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    /// <summary>
    /// Class that connects data to the register view.
    /// </summary>
    public class RegisterViewModel
    {
        #region User properties
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
        #endregion

        #region Student specific properties
        private int _yearOfStudy;

        #region Getters & Setters
        [Required(ErrorMessage ="Anul de studiu trebuie sa fie selectat")]
        [Display(Name="Anul de studiu")]
        public int YearOfStudy
        {
            get => _yearOfStudy;
            set => _yearOfStudy = value;
        }
        #endregion
        #endregion

        #region Professor specific properties
        private string _department;

        #region Getters & Setters
        [Required(ErrorMessage = "Departamentul trebuie sa fie selectat")]
        [Display(Name="Departament")]
        public string Department
        {
            get => _department;
            set => _department = value;
        }
        #endregion
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class CreateProfessorUserViewModel : CreateUserViewModel
    {
        #region Private properties
        private string _department;
        #endregion

        #region Getters & Setters
        [Required(ErrorMessage ="Departamentul este necesar")]
        [Display(Name="Departament")]
        public string Department
        {
            get => _department;
            set => _department = value;
        }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class CreateStudentUserViewModel : CreateUserViewModel
    {
        #region Private properties
        private int _yearOfStudy;
        #endregion

        #region Getters & Setters
        [Required(ErrorMessage ="Anul de studiu este necesar")]
        [Display(Name ="Anul de studiu")]
        public int YearOfStudy
        {
            get => _yearOfStudy;
            set => _yearOfStudy = value;
        }
        #endregion
    }
}

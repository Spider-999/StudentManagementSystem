using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class CreateQuizViewModel
    {
        #region Private properties
        private string _title;
        private string _description;
        public List<CreateQuizQuestionViewModel> _questions;
        #endregion

        #region Constructor
        public CreateQuizViewModel()
        {
            Questions = new List<CreateQuizQuestionViewModel>();
        }
        #endregion

        #region Getters & Setters
        [Required]
        [Display(Name ="Titlu")]
        public string Title
        {
            get => _title;
            set => _title = value;  
        }
        [Required]
        [Display(Name = "Cerinta")]
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        [Required]
        [Display(Name = "Intrebari")]
        public List<CreateQuizQuestionViewModel> Questions
        {
            get => _questions;
            set => _questions = value;
        }
        #endregion
    }
}

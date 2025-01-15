using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class CreateQuizQuestionViewModel
    {
        #region Private properties
        private string _question;
        private List<string> _answers;
        public string _correctAnswer;
        #endregion

        #region Getters & Setters
        [Required(ErrorMessage = "Intrebarile sunt necesare")]
        [Display(Name ="Intrebare")]
        public string Question
        {
            get => _question; 
            set => _question = value;
        }
        [Required(ErrorMessage ="Raspunsurile sunt necesare")]
        [Display(Name = "Raspunsuri")]
        public List<string> Answers
        {
            get => _answers;
            set => _answers = value;
        }
        [Required(ErrorMessage = "Raspunsul corect este necesar")]
        [Display(Name = "Raspuns corect")]
        public string CorrectAnswer
        {
            get => _correctAnswer;
            set => _correctAnswer = value;
        }
        #endregion
    }
}

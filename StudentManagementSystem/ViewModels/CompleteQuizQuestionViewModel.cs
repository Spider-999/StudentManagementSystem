using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class CompleteQuizQuestionViewModel
    {
        #region Private properties
        private string _question;
        private List<string> _answers;
        private string _selectedAnswer;
        #endregion

        public CompleteQuizQuestionViewModel()
        {
            _answers = new List<string>();
        }

        #region Getters & Setters
        public string Question
        {
            get => _question;
            set => _question = value;
        }

        public List<string> Answers
        {
            get => _answers;
            set => _answers = value;
        }

        [Required(ErrorMessage = "Selecteaza raspunsul")]
        public string SelectedAnswer
        {
            get => _selectedAnswer;
            set => _selectedAnswer = value;
        }
        #endregion
    }
}

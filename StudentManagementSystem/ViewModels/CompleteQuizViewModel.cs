namespace StudentManagementSystem.ViewModels
{
    public class CompleteQuizViewModel
    {
        #region Private properties
        private string _quizID;
        private string _title;
        private List<CompleteQuizQuestionViewModel> _questions;
        private int _timeLimit;
        #endregion

        #region Constructor
        public CompleteQuizViewModel()
        {
            Questions = new List<CompleteQuizQuestionViewModel>();
        }
        #endregion

        #region Getters & Setters
        public string QuizID
        {
            get => _quizID;
            set => _quizID = value;
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public List<CompleteQuizQuestionViewModel> Questions
        {
            get => _questions;
            set => _questions = value;
        }

        public int TimeLimit
        {
            get => _timeLimit;
            set => _timeLimit = value;
        }
        #endregion
    }
}

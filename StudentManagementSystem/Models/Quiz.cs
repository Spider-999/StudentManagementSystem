namespace StudentManagementSystem.Models
{
    public class Quiz : Homework
    {
        #region Private properties
        private ICollection<QuizQuestion> _quizQuestions;
        private int _timeLimit;
        #endregion

        #region Getters & Setters
        public ICollection<QuizQuestion> QuizQuestions
        {
            get => _quizQuestions;
            set => _quizQuestions = value;
        }
        public int TimeLimit
        {
            get => _timeLimit;
            set => _timeLimit = value;
        }
        #endregion
    }
}

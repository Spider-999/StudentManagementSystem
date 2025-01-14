namespace StudentManagementSystem.Models
{
    public class Quiz : Homework
    {
        #region Private properties
        private ICollection<QuizQuestion> _quizQuestions;
        #endregion

        #region Getters & Setters
        public ICollection<QuizQuestion> QuizQuestions
        {
            get => _quizQuestions;
            set => _quizQuestions = value;
        }
        #endregion
    }
}

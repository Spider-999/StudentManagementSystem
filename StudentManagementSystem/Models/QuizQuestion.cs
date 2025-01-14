using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StudentManagementSystem.Models
{
    /// <summary>
    /// Each Quiz homework can have main quiz questions.
    /// This class is used in Quiz.cs.
    /// </summary>
    public class QuizQuestion
    {
        #region Private properties
        private string _id;
        private string _question;
        private List<string> _answers;
        private string _correctAnswer;
        private string _quizID;
        private Quiz _quiz;
        #endregion

        #region Getters & Setters
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id
        {
            get => _id;
            set => _id = value;
        }

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

        public string CorrectAnswer
        {
            get => _correctAnswer;
            set => _correctAnswer = value;
        }

        public string QuizID
        {
            get => _quizID;
            set => _quizID = value;
        }

        [ForeignKey("QuizID")]
        public Quiz Quiz
        {
            get => _quiz;
            set => _quiz = value;
        }
        #endregion
    }
}

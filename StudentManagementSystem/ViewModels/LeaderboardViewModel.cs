namespace StudentManagementSystem.ViewModels
{
    public class LeaderboardViewModel
    {
        #region Private properties
        private string _studentName;
        private int _score;
        #endregion

        #region Getters & Setters
        public string StudentName
        {
            get => _studentName;
            set => _studentName = value;
        }

        public int Score
        {
            get => _score;
            set => _score = value;
        }
        #endregion
    }
}

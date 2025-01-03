namespace StudentManagementSystem.Models
{
    public class Discipline
    {
        #region Private properties
        private string _name;
        private double _gradeAverage;
        #endregion

        #region Getters & Setters
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public double GradeAverage
        {
            get => _gradeAverage;
            set => _gradeAverage = value;
        }
        #endregion

        #region Database relationships
        // A discipline can have many homeworks
        #region Private properties
        private ICollection<Homework> _homeworks;
        #endregion

        #region Getters & Setters
        public ICollection<Homework> Homeworks
        {
            get => _homeworks;
            set => _homeworks = value;
        }
        #endregion
        #endregion
    }
}

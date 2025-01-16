namespace StudentManagementSystem.ViewModels
{
    public class DisciplineGradeViewModel
    {
        #region Private properties
        private string _disciplineName;
        private double? _gradeAverage;
        private double? _generalGrade;
        #endregion

        #region Getters & Setters
        public string DisciplineName
        {
            get => _disciplineName;
            set => _disciplineName = value;
        }

        public double? GradeAverage
        {
            get => _gradeAverage;
            set => _gradeAverage = value;
        }

        public double? GeneralGrade
        {
            get => _generalGrade;
            set => _generalGrade = value;
        }
        #endregion
    }
}

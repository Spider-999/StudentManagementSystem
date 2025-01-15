namespace StudentManagementSystem.ViewModels
{
    public class StudentGradesViewModel
    {
        #region Private properties
        private string _studentName;
        private List<DisciplineGradeViewModel> _disciplines;
        #endregion

        #region Getters & Setters
        public string StudentName
        {
            get => _studentName;
            set => _studentName = value;
        }

        public List<DisciplineGradeViewModel> Disciplines
        {
            get => _disciplines;
            set => _disciplines = value;
        }
        #endregion
    }
}

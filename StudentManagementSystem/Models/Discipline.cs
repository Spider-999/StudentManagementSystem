namespace StudentManagementSystem.Models
{
    public class Discipline
    {
        #region Private properties
        private string _id;
        private string _name;
        private double? _gradeAverage;
        // Default calculation formula
        private string? _gradeCalculationFormula = "MA1";
        #endregion

        #region Getters & Setters
        public string Id
        {
            get => _id;
            set => _id = value;
        }
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public double? GradeAverage
        {
            get => _gradeAverage;
            set => _gradeAverage = value;
        }
        public string? GradeCalculationFormula
        {
            get => _gradeCalculationFormula;
            set => _gradeCalculationFormula = value;
        }
        #endregion

        #region Database relationships
        // A discipline can have many homeworks,many professors and many students
        #region Private properties
        private ICollection<Homework> _homeworks;
        private ICollection<Professor> _professors;
        private ICollection<StudentDiscipline> _studentDisciplines;
        #endregion

        #region Getters & Setters
        public ICollection<Homework> Homeworks
        {
            get => _homeworks;
            set => _homeworks = value;
        }
        public ICollection<Professor> Professors
        {
            get => _professors;
            set => _professors = value;
        }
        public ICollection<StudentDiscipline> StudentDisciplines
        {
            get => _studentDisciplines;
            set => _studentDisciplines = value;
        }
        #endregion
        #endregion


    }
}

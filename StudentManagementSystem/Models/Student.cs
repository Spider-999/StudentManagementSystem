namespace StudentManagementSystem.Models
{
    public class Student : User
    {
        #region Private properties
        private int _yearOfStudy;
        private double? _generalGrade;
       
        #endregion

        #region Getters & Setters
        public int YearOfStudy
        {
            get => _yearOfStudy;
            set => _yearOfStudy = value;
        }

        public double? GeneralGrade
        {
            get => _generalGrade;
            set => _generalGrade = value;
        }
        #endregion

        #region Database relationships
        // A student can have many disciplines
        #region Private properties
        private ICollection<StudentDiscipline> _studentDisciplines;
        private ICollection<Homework> _homeworks;
        #endregion

        #region Getters & Setters
        public ICollection<StudentDiscipline> StudentDisciplines
        {
            get => _studentDisciplines;
            set => _studentDisciplines = value;
        }

        public ICollection<Homework> Homeworks
        {
            get => _homeworks;
            set => _homeworks = value;
        }
        #endregion
        #endregion
    }
}

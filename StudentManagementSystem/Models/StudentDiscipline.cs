namespace StudentManagementSystem.Models
{
    public class StudentDiscipline
    {
        //Intermediary class for the many to many relationship between Student and Discipline

        #region Private properties
        private string _studentId;
        private Student _student;
        private string _disciplineId;
        private Discipline _discipline;
        #endregion

        #region Getters & Setters
        public string StudentId
        {
            get => _studentId;
            set => _studentId = value;
        }
        public Student Student
        {
            get => _student;
            set => _student = value;
        }
        public string DisciplineId
        {
            get => _disciplineId;
            set => _disciplineId = value;
        }
        public Discipline Discipline
        {
            get => _discipline;
            set => _discipline = value;
        }
        #endregion
    }
}

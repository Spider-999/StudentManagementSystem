namespace StudentManagementSystem.Models
{
    public class Student : User
    {
        #region Private properties
        private string _yearOfStudy;
        private double _generalGrade;
        #endregion

        #region Getters & Setters
        public string YearOfStudy
        {
            get => _yearOfStudy;
            set => _yearOfStudy = value;
        }

        public double GeneralGrade
        {
            get => _generalGrade;
            set => _generalGrade = value;
        }
        #endregion
    }
}

namespace StudentManagementSystem.Models
{
    public class Professor : User
    {
        #region Private properties
        private string _department;
        #endregion

        #region Getters & Setters
        public string Department
        {
            get => _department;
            set => _department = value;
        }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations.Schema;

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

        #region Database relationships
        //A professor can have only one discipline
        #region Private properties
        private string _disciplineId;
        private Discipline _discipline;
        #endregion

        #region Getters & Setters
        public string DisciplineId
        {
            get => _disciplineId;
            set => _disciplineId = value;
        }
        [ForeignKey("DisciplineId")]
        public Discipline Discipline
        {
            get => _discipline;
            set => _discipline = value;
        }
        #endregion

        #endregion
    }
}

namespace StudentManagementSystem.Models
{
    public class Homework
    {
        #region Private properties
        private string _title;
        private string _description;
        private DateTime _creationDate;
        private DateTime _endDate;
        private double _grade;
        private bool _status;
        private bool _mandatory;
        private double _penalty;
        private bool _afterEndDateUpload;
        #endregion

        #region Getters & Setters
        public string Title
        {
            get => _title;
            set => _title = value;
        }
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        public DateTime CreationDate
        {
            get => _creationDate;
            set => _creationDate = value;
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }
        public double Grade
        {
            get => _grade;
            set => _grade = value;
        }
        public bool Status
        {
            get => _status;
            set => _status = value;
        }
        public bool Mandatory
        {
            get => _mandatory;
            set => _mandatory = value;
        }
        public double Penalty
        {
            get => _penalty;
            set => _penalty = value;
        }
        public bool AfterEndDateUpload
        {
            get => _afterEndDateUpload;
            set => _afterEndDateUpload = value;
        }
        #endregion

        #region Database relationships
        // Homeworks are hooked up to different disciplines
        #region Private properties
        private Discipline _discipline;
        private int _disciplineId;

        public Discipline Discipline
        {
            get => _discipline;
            set => _discipline = value;
        }
        public int DisciplineId
        {
            get => _disciplineId;
            set => _disciplineId = value;
        }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Homework
    {
        #region Private properties
        private string _id;
        private string? _studentId;
        private Student? _student;
        private string? _title;
        private string? _description;
        private string _content;
        private DateTime? _creationDate;
        private DateTime? _endDate;
        private double? _grade;
        private bool? _status;
        private bool? _mandatory;
        private double? _penalty;
        private bool? _afterEndDateUpload;
        private bool? _isTemplate;
        #endregion

        #region Getters & Setters
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id
        {
            get => _id;
            set => _id = value;
        }
        public string? Title
        {
            get => _title;
            set => _title = value;
        }
        public string? Description
        {
            get => _description;
            set => _description = value;
        }
        public DateTime? CreationDate
        {
            get => _creationDate;
            set => _creationDate = value;
        }
        public DateTime? EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }
        public double? Grade
        {
            get => _grade;
            set => _grade = value;
        }
        public bool? Status
        {
            get => _status;
            set => _status = value;
        }
        public bool? Mandatory
        {
            get => _mandatory;
            set => _mandatory = value;
        }
        public double? Penalty
        {
            get => _penalty;
            set => _penalty = value;
        }
        public bool? AfterEndDateUpload
        {
            get => _afterEndDateUpload;
            set => _afterEndDateUpload = value;
        }

        public string? Content
        {
            get => _content;
            set => _content = value;
        }

        public bool? IsTemplate
        {
            get => _isTemplate;
            set => _isTemplate = value;
        }
        #endregion

        #region Database relationships
        // Homeworks are hooked up to different disciplines
        #region Private properties
        private Discipline _discipline;
        private string _disciplineId;
        #endregion

        #region Getters & Setters
        public string? DisciplineId
        {
            get => _disciplineId;
            set => _disciplineId = value;
        }
        [ForeignKey("DisciplineId")]
        public Discipline? Discipline
        {
            get => _discipline;
            set => _discipline = value;
        }
        public string? StudentId
        {
            get => _studentId;
            set => _studentId = value;
        }
        [ForeignKey("StudentId")]
        public Student? Student
        {
            get => _student;
        }
        #endregion
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class HomeworkViewModel
    {
        #region Private properties
        private string? _id;
        private string? _title;
        private string? _description;
        private string? _content;
        private DateTime? _creationDate;
        private DateTime? _endDate;
        private double? _grade;
        private bool? _status;
        private bool? _mandatory;
        private double? _penalty;
        private bool? _afterEndDateUpload;
        private bool? _hasGrade;
        private string? _homeworkType;
        #endregion

        #region Getters & setters
        public string? Id
        {
            get => _id;
            set => _id = value;
        }
        [Display(Name ="Titlu")]
        public string? Title
        {
            get => _title;
            set => _title = value;
        }

        [Display(Name = "Cerinta")]
        public string? Description
        {
            get => _description;
            set => _description = value;
        }

        [Display(Name = "Continut")]
        public string? Content
        {
            get => _content;
            set => _content = value;
        }

        [Display(Name = "Data creeari")]
        public DateTime? CreationDate
        {
            get => _creationDate;
            set => _creationDate = value;
        }

        [Display(Name = "Termenul limita")]
        public DateTime? EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        [Display(Name = "Nota")]
        public double? Grade
        {
            get => _grade;
            set => _grade = value;
        }

        [Display(Name = "Stare")]
        public bool? Status
        {
            get => _status;
            set => _status = value;
        }

        [Display(Name ="Tema obligatorie")]
        public bool? Mandatory
        {
            get => _mandatory;
            set => _mandatory = value;
        }

        [Display(Name ="Penalizare pentru intarzierea upload-ului")]
        public double? Penalty
        {
            get => _penalty;
            set => _penalty = value;
        }

        [Display(Name ="Poate fi uploadata tema dupa termenul limita?")]
        public bool? AfterEndUploadDate
        {
            get => _afterEndDateUpload;
            set => _afterEndDateUpload = value;
        }

        [Display(Name = "Nota")]
        public bool? HasGrade
        {
            get => _hasGrade;
            set => _hasGrade = value;
        }

        [Display(Name ="Tipul temei")]
        public string? HomeworkType
        {
            get => _homeworkType;
            set => _homeworkType = value;
        }
        #endregion
    }
}

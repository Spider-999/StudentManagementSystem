using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace StudentManagementSystem.ViewModels
{
    public class CreateQuizViewModel
    {
        #region Private properties
        private string _title;
        private string _description;
        private DateTime? _creationDate;
        private DateTime? _endDate;
        private double? _grade;
        private bool? _status;
        private bool? _mandatory;
        private double? _penalty;
        private bool? _afterEndDateUpload;
        private string? _homeworkType;
        public List<CreateQuizQuestionViewModel> _questions;
        #endregion

        #region Constructor
        public CreateQuizViewModel()
        {
            Questions = new List<CreateQuizQuestionViewModel>();
        }
        #endregion

        #region Getters & Setters
        [Required(ErrorMessage ="Titlul este necesar")]
        [Display(Name ="Titlu")]
        public string Title
        {
            get => _title;
            set => _title = value;  
        }
        [Required(ErrorMessage = "Cerinta este necesara")]
        [Display(Name = "Cerinta")]
        public string Description
        {
            get => _description;
            set => _description = value;
        }
        [Required(ErrorMessage = "Intrebarile sunt necesare")]
        [Display(Name = "Intrebari")]
        public List<CreateQuizQuestionViewModel> Questions
        {
            get => _questions;
            set => _questions = value;
        }

        [Display(Name = "Data creeari")]
        public DateTime? CreationDate
        {
            get => _creationDate;
            set => _creationDate = value;
        }
        [Required(ErrorMessage = "Termenul limita este necesar")]
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
        [Required(ErrorMessage = "Starea este necesara")]
        [Display(Name = "Stare")]
        public bool? Status
        {
            get => _status;
            set => _status = value;
        }
        [Required(ErrorMessage = "Setarea optiunii este necesara")]
        [Display(Name = "Test obligatoriu")]
        public bool? Mandatory
        {
            get => _mandatory;
            set => _mandatory = value;
        }
        [Required(ErrorMessage = "Setarea penalizarii este necesara")]
        [Display(Name = "Penalizare pentru intarzierea upload-ului")]
        public double? Penalty
        {
            get => _penalty;
            set => _penalty = value;
        }
        [Required(ErrorMessage = "Setarea optiunii este necesara")]
        [Display(Name = "Poate fi uploadata tema dupa termenul limita?")]
        public bool? AfterEndUploadDate
        {
            get => _afterEndDateUpload;
            set => _afterEndDateUpload = value;
        }

        [Display(Name = "Tipul temei")]
        public string? HomeworkType
        {
            get => _homeworkType;
            set => _homeworkType = value;
        }
        #endregion
    }
}

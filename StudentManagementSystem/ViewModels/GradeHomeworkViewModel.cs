using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class GradeHomeworkViewModel
    {
        #region Private properties
        private string _id;
        private string? _title;
        private string? _description;
        private string? _content;
        private double _grade;
        private string? _comment;
        #endregion

        #region Getters & Setters
        public string Id
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
        [Display(Name ="Cerinta")]
        public string? Description
        {
            get => _description;
            set => _description = value;
        }
        public string? Content
        {
            get => _content;
            set => _content = value;
        }
        [Required(ErrorMessage ="Nota este necesara")]
        [Range(1, 10, ErrorMessage="Nota trebuie sa fie intre 1 si 10")]
        [Display(Name ="Nota")]
        public double Grade
        {
            get => _grade;
            set => _grade = value;
        }
        [Display(Name ="Comentariu")]
        public string? Comment
        {
            get => _comment;
            set => _comment = value;
        }
        #endregion
    }
}

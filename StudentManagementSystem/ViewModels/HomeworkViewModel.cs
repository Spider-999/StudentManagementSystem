using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class HomeworkViewModel
    {
        #region Private properties
        private string _title;
        private string _description;
        private string _content;
        private DateTime _creationDate;
        private DateTime _endDate;
        private double _grade;
        private bool _status;
        private bool _mandatory;
        private double _penalty;
        private bool _afterEndDateUpload;
        #endregion

        #region Getters & setters
        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        #endregion
    }
}

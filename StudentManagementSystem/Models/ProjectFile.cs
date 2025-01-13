using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class ProjectFile
    {
        #region Private properties
        private string _id;
        private string _fileName;
        private string _filePath;
        private string _homeworkID;
        private Project homework;
        #endregion

        #region Getters & Setters
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        [Required]
        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }

        [Required]
        public string FilePath
        {
            get => _filePath;
            set => _filePath = value;
        }
        #endregion

        #region Database relationships
        public string HomeworkID
        {
            get => _homeworkID;
            set => _homeworkID = value;
        }

        [ForeignKey("HomeworkID")]
        public Project Homework
        {
            get => homework;
            set => homework = value;
        }
        #endregion
    }
}

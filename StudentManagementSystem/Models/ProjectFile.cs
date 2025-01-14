using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class ProjectFile
    {
        #region Private properties
        private string _id;
        private string _fileName;
        // An array of bytes to store the file in the DB
        private byte[] _fileContent;
        private string _projectID;
        private Project _project;
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
        public byte[] FileContent
        {
            get => _fileContent;
            set => _fileContent = value;
        }
        #endregion

        #region Database relationships
        public string ProjectID
        {
            get => _projectID;
            set => _projectID = value;
        }

        [ForeignKey("ProjectID")]
        public Project Project
        {
            get => _project;
            set => _project = value;
        }
        #endregion
    }
}

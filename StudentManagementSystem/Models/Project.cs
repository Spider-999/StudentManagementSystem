namespace StudentManagementSystem.Models
{
    /// <summary>
    /// This class is a type of homework where the user
    /// can upload one file or more and the professor can view 
    /// the file/files and grade the project.
    /// </summary>
    public class Project : Homework
    {
        #region Private properties
        private ICollection<ProjectFile> _projectFiles;
        #endregion

        #region Getters & Setters
        public ICollection<ProjectFile> ProjectFiles
        {
            get => _projectFiles;
            set => _projectFiles = value;
        }
        #endregion
    }
}

namespace StudentManagementSystem.ViewModels
{
    public class ProjectFileUploadViewModel
    {
        #region Private properties
        private string? _projectID;
        // File sent through post request
        private List<IFormFile> _files;
        #endregion

        #region Getters & Setters
        public string? ProjectID
        {
            get => _projectID;
            set => _projectID = value;
        }
        public List<IFormFile> Files
        {
            get => _files;
            set => _files = value;
        }
        #endregion
    }
}

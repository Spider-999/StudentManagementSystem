using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.ViewModels
{
    public class GradeCalculationFormulaViewModel
    {
        #region Private properties
        private string _disciplineID;
        private string _formula;
        #endregion

        #region Getters & Setters
        public string DisciplineID
        {
            get => _disciplineID;
            set => _disciplineID = value;
        }
        [Required(ErrorMessage ="Formula de calcul este necesara")]
        [Display(Name ="Formula de calcul")]
        public string Formula
        {
            get => _formula;
            set => _formula = value;
        }
        #endregion
    }
}

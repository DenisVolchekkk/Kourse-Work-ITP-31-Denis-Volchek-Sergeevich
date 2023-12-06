using Univercity.Domain;
using System.ComponentModel.DataAnnotations;

namespace Univercity.Application.ViewModels
{
    public class StudentsGroupViewModel
    {
        public IEnumerable<StudentsGroup> StudentsGroups { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        [Display(Name = "Группа")]
        [Required(ErrorMessage = "Не названа группа")]
        public string NumberOfGroup { get; set; } = null!;
        [Display(Name = "Кол-во студентов")]
        [Required(ErrorMessage = "Не указано кол-во студентов")]
        public int? QuantityOfStudents { get; set; }
        [Display(Name = "Факультет")]
        public string Facility { get; set; }

    }
}

using Univercity.Domain;
using System.ComponentModel.DataAnnotations;

namespace Univercity.Application.ViewModels
{
    public class DisciplineTypeViewModel
    {
        public IEnumerable<DisciplineType> DisciplineTypes { get; set; }

        [Display(Name = "Тип дисциплины")]
        [Required(ErrorMessage = "Не указан тип дисциплины")]
        public string TypeOfDiscipline { get; set; } = null!;
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

    }
}

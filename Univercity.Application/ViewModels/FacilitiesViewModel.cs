using Univercity.Domain;
using System.ComponentModel.DataAnnotations;

namespace Univercity.Application.ViewModels
{
    public class FacilitiesViewModel
    {
        public IEnumerable<Facility> Facilities { get; set; }

        [Display(Name = "Факультет")]
        [Required(ErrorMessage = "Не указан факультет")]
        public string FacilityName { get; set; } = null!;
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

    }
}

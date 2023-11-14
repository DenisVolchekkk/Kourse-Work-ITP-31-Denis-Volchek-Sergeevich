using Study.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Study.ViewModels
{
    public class DisciplinesViewModel
    {
        public IEnumerable<Discipline> Disciplines { get; set; }
        [Display(Name = "Дисциплина")]
        [Required(ErrorMessage = "Не указана дисциплина")]
        public string DisciplineName { get; set; } = null!;
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }


    }
}

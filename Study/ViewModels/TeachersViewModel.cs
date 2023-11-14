using Study.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Study.ViewModels
{
    public class TeachersViewModel
    {
        public IEnumerable<Teacher> Teachers { get; set; }
        [Display(Name = "Имя учителя")]
        [Required(ErrorMessage = "Не указано имя учителя")]
        public string TeacherName { get; set; } = null!;
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }


    }
}

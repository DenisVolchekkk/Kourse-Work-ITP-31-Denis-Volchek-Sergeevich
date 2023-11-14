using Study.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Study.ViewModels
{
    public class LessonTimesViewModel
    {
        public IEnumerable<LessonsTime> LessonsTimes { get; set; }

        [Display(Name = "Время")]
        [Required(ErrorMessage = "Не указано время")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]

        [DataType(DataType.Time)]

        public TimeSpan LessonTime { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }

    }
}

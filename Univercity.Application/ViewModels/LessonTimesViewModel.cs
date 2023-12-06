using Univercity.Domain;
using System.ComponentModel.DataAnnotations;

namespace Univercity.Application.ViewModels
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

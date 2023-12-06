using Study.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Universaty.Application.ViewModels
{
    public class ClassroomsViewModel
    {
        public IEnumerable<Classroom> Classrooms { get; set; }

        [Display(Name = "Номер аудитории")]
        public int NumberOfClassroom { get; set; }

        [Display(Name = "Кол-во мест")]
        public int Places { get; set; }
        [Display(Name = "Время")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        [DataType(DataType.Time)]
        public TimeSpan LessonTime { get; set; }

        [Display(Name = "Крыло")]
        public int Wing { get; set; }

        [Display(Name = "Тип аудитории")]
        public string ClassroomType { get; set; } = null!;
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime? LessonDate { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }


    }
}

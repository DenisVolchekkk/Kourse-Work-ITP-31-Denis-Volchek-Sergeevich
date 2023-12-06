using Microsoft.AspNetCore.Mvc.Rendering;
using Study.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Study.ViewModels
{
    public class LessonsViewModel
    {    // Добавьте свойства для хранения значений из формы

        public IEnumerable<Lesson> Lessons { get; set; }
        //public SelectList LessonTimes { get; set; } = new SelectList(new List<LessonsTime>(), "LessonTimeId", "LessonTime");
       // public SelectList Disciplines { get; set; } = new SelectList(new List<Discipline>(), "DisciplineId", "DisciplineName");
        public FilterLessonViewModel FilterLessonViewModel { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        public SortViewModel SortViewModel { get; set; }
    }
}

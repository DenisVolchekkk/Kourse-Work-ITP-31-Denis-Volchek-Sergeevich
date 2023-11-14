using Univercity.Models;

namespace Univercity.ViewModels
{
    public class LessonViewModel
    {
        public IEnumerable<Lesson> Lessons { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
    }
}

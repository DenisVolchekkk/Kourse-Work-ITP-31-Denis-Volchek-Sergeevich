using System.ComponentModel.DataAnnotations;
using System;


namespace Study.ViewModels
{
    public class FilterLessonViewModel
    {
        public int LessonId { get; set; }
        [Display(Name = "Дисциплина")]
        public string Discipline { get; set; }
        [Display(Name = "Тип дисциплины")]
        public string DisciplineType { get; set; }
        [Display(Name = "Факультет")]
        public string Facility { get; set; }
        [Display(Name = "Аудитория")]
        public int Classroom { get; set; }
        [Display(Name = "Время")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]
        [DataType(DataType.Time)]
        public TimeSpan LessonTime { get; set; }
        [Display(Name = "Учитель")]
        public string Teacher { get; set; }
        [Display(Name = "Группа")]
        public string StudentGroup { get; set; }
        [Display(Name = "Семестр")]

        public int Semestr { get; set; }
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime? LessonDate { get; set; }

        [Display(Name = "Год")]

        public int Year { get; set; }
        [Display(Name = "День недели")]
        public int DayOfweek { get; set; }
        [Display(Name = "Тип аудитории")]
        public string ClassroomType { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Study.Models;

public partial class LessonsTime
{
    [Key]
    [Display(Name = "Код времени урока")]
    public int LessonTimeId { get; set; }

    [Display(Name = "Время")]
    [Required(ErrorMessage = "Не указано время")]
    [DisplayFormat(DataFormatString = "{0:hh\\:mm}")]

    [DataType(DataType.Time)]

    public TimeSpan LessonTime { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

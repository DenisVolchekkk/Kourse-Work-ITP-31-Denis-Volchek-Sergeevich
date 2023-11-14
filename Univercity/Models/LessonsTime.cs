using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Univercity.Models;

public partial class LessonsTime
{
    [Key]
    public int LessonTimeId { get; set; }

    public TimeSpan LessonTime { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

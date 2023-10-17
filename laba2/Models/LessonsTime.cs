using System;
using System.Collections.Generic;

namespace laba2.Models;

public partial class LessonsTime
{
    public int LessonTimeId { get; set; }

    public TimeSpan LessonTime { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

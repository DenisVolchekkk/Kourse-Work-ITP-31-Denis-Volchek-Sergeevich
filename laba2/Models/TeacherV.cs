using System;
using System.Collections.Generic;

namespace laba2.Models;

public partial class TeacherV
{
    public int? Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisciplineName { get; set; } = null!;

    public string DisciplineType { get; set; } = null!;

    public DateTime? LessonDate { get; set; }

    public TimeSpan LessonTime { get; set; }
}

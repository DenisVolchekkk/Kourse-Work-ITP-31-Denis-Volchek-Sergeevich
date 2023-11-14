using System;
using System.Collections.Generic;

namespace Univercity.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int? DisciplineId { get; set; }

    public int? ClassroomId { get; set; }

    public int? DisciplineTypeId { get; set; }

    public int? TeacherId { get; set; }

    public int? StudentsGroupId { get; set; }

    public int? Semestr { get; set; }

    public DateTime? LessonDate { get; set; }

    public int? LessonTimeId { get; set; }

    public int? Year { get; set; }

    public int? DayOfweek { get; set; }

    public virtual Classroom? Classroom { get; set; }

    public virtual Discipline? Discipline { get; set; }

    public virtual DisciplineType? DisciplineType { get; set; }

    public virtual LessonsTime? LessonTime { get; set; }

    public virtual StudentsGroup? StudentsGroup { get; set; }

    public virtual Teacher? Teacher { get; set; }
}

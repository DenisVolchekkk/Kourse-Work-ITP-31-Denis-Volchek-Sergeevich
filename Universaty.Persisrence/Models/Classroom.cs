using System;
using System.Collections.Generic;

namespace Universaty.Persisrence.Models;

public partial class Classroom
{
    public int ClassroomId { get; set; }

    public int NumberOfClassroom { get; set; }

    public int Places { get; set; }

    public int Wing { get; set; }

    public string ClassroomType { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

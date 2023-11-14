using System;
using System.Collections.Generic;

namespace Laba3.Models;

public partial class DisciplineType
{
    public int DisciplineTypeId { get; set; }

    public string TypeOfDiscipline { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

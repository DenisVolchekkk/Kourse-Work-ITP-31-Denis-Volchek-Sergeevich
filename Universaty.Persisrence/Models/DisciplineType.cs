using System;
using System.Collections.Generic;

namespace Universaty.Persisrence.Models;

public partial class DisciplineType
{
    public int DisciplineTypeId { get; set; }

    public string DisciplineType1 { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

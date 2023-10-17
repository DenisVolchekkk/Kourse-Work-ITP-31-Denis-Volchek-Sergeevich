using System;
using System.Collections.Generic;

namespace laba2.Models;

public partial class StudentsGroup
{
    public int StudentsGroupId { get; set; }

    public string NumberOfGroup { get; set; } = null!;

    public int? QuantityOfStudents { get; set; }

    public int? FacilityId { get; set; }

    public virtual Facility? Facility { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

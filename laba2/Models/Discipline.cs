﻿using System;
using System.Collections.Generic;

namespace laba2.Models;

public partial class Discipline
{
    public int DisciplineId { get; set; }

    public string DisciplineName { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
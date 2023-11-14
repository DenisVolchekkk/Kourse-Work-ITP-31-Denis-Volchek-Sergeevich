using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Univercity.Models;

public partial class LessonsDbContext : DbContext
{
    public LessonsDbContext()
    {
    }

    public LessonsDbContext(DbContextOptions<LessonsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Classroom> Classrooms { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<DisciplineType> DisciplineTypes { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonsTime> LessonsTimes { get; set; }

    public virtual DbSet<StudentsGroup> StudentsGroups { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }


}

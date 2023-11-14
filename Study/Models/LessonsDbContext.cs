using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Study.Models;

public partial class LessonsDbContext : DbContext
{

    public LessonsDbContext(DbContextOptions<LessonsDbContext> options)
        : base(options)
    {
    }

    public  DbSet<Classroom> Classrooms { get; set; }

    public  DbSet<Discipline> Disciplines { get; set; }

    public  DbSet<DisciplineType> DisciplineTypes { get; set; }

    public  DbSet<Facility> Facilities { get; set; }

    public  DbSet<Lesson> Lessons { get; set; }

    public  DbSet<LessonsTime> LessonsTimes { get; set; }

    public  DbSet<StudentsGroup> StudentsGroups { get; set; }

    public  DbSet<Teacher> Teachers { get; set; }


}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universaty.Domain
{

    public class SchoolContext : DbContext
    {
        public DbSet<LessonsTime> LessonsTimes { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<DisciplineTyp> DisciplineTypes { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<StudentsGroup> StudentsGroups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("UniversatyConnection"));
        }

    }
}

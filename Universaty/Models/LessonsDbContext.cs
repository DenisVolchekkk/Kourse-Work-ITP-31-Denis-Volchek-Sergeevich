using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Laba3.Models;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-AKEGVAH;Database=LessonsDB;Integrated Security=SSPI;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PK__Classroo__11618E8A6ACF3628");

            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.ClassroomType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.DisciplineId).HasName("PK__Discipli__29093750B5FECEBA");

            entity.Property(e => e.DisciplineId).HasColumnName("DisciplineID");
            entity.Property(e => e.DisciplineName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DisciplineType>(entity =>
        {
            entity.HasKey(e => e.DisciplineTypeId).HasName("PK__Discipli__3863BB44C7D80B69");

            entity.Property(e => e.DisciplineTypeId).HasColumnName("DisciplineTypeID");
            entity.Property(e => e.TypeOfDiscipline)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Faciliti__5FB08B945C8C5FB9");

            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.FacilityName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACB0DF00A821");

            entity.Property(e => e.LessonId).HasColumnName("LessonID");
            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.DisciplineId).HasColumnName("DisciplineID");
            entity.Property(e => e.DisciplineTypeId).HasColumnName("DisciplineTypeID");
            entity.Property(e => e.LessonDate).HasColumnType("date");
            entity.Property(e => e.LessonTimeId).HasColumnName("LessonTimeID");
            entity.Property(e => e.StudentsGroupId).HasColumnName("StudentsGroupID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Classroom).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.ClassroomId)
                .HasConstraintName("FK__Lessons__Classro__102C51FF");

            entity.HasOne(d => d.Discipline).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.DisciplineId)
                .HasConstraintName("FK__Lessons__Discipl__11207638");

            entity.HasOne(d => d.DisciplineType).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.DisciplineTypeId)
                .HasConstraintName("FK__Lessons__Discipl__12149A71");

            entity.HasOne(d => d.LessonTime).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.LessonTimeId)
                .HasConstraintName("FK__Lessons__LessonT__0F382DC6");

            entity.HasOne(d => d.StudentsGroup).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.StudentsGroupId)
                .HasConstraintName("FK__Lessons__Student__13FCE2E3");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Lessons__Teacher__1308BEAA");
        });

        modelBuilder.Entity<LessonsTime>(entity =>
        {
            entity.HasKey(e => e.LessonTimeId).HasName("PK__LessonsT__D0A07B8A1FA062A6");

            entity.Property(e => e.LessonTimeId).HasColumnName("LessonTimeID");
        });

        modelBuilder.Entity<StudentsGroup>(entity =>
        {
            entity.HasKey(e => e.StudentsGroupId).HasName("PK__Students__24EA6103D9D5BB72");

            entity.Property(e => e.StudentsGroupId).HasColumnName("StudentsGroupID");
            entity.Property(e => e.NumberOfGroup)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Facility).WithMany(p => p.StudentsGroups)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__StudentsG__Facil__0C5BC11B");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259447E65E044");

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.TeacherName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace laba2.Models;

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

    public virtual DbSet<FacilityV> FacilityVs { get; set; }

    public virtual DbSet<GroupV> GroupVs { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonsTime> LessonsTimes { get; set; }

    public virtual DbSet<StudentsGroup> StudentsGroups { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherV> TeacherVs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-AKEGVAH;Database=LessonsDB;Integrated Security=SSPI;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Classroom>(entity =>
        {
            entity.HasKey(e => e.ClassroomId).HasName("PK__Classroo__11618E8AB3036696");

            entity.Property(e => e.ClassroomId).HasColumnName("ClassroomID");
            entity.Property(e => e.ClassroomType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.DisciplineId).HasName("PK__Discipli__29093750261EE449");

            entity.Property(e => e.DisciplineId).HasColumnName("DisciplineID");
            entity.Property(e => e.DisciplineName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DisciplineType>(entity =>
        {
            entity.HasKey(e => e.DisciplineTypeId).HasName("PK__Discipli__3863BB44F3F2397B");

            entity.Property(e => e.DisciplineTypeId).HasColumnName("DisciplineTypeID");
            entity.Property(e => e.DisciplineType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DisciplineType");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Faciliti__5FB08B9486E5B8D6");

            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.FacilityName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FacilityV>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Facility_v");

            entity.Property(e => e.DisciplineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DisciplineType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LessonDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GroupV>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Group_v");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACB049ECADED");

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
                .HasConstraintName("FK__Lessons__Classro__6E0C4425");

            entity.HasOne(d => d.Discipline).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.DisciplineId)
                .HasConstraintName("FK__Lessons__Discipl__6F00685E");

            entity.HasOne(d => d.DisciplineType).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.DisciplineTypeId)
                .HasConstraintName("FK__Lessons__Discipl__6FF48C97");

            entity.HasOne(d => d.LessonTime).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.LessonTimeId)
                .HasConstraintName("FK__Lessons__LessonT__6D181FEC");

            entity.HasOne(d => d.StudentsGroup).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.StudentsGroupId)
                .HasConstraintName("FK__Lessons__Student__71DCD509");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK__Lessons__Teacher__70E8B0D0");
        });

        modelBuilder.Entity<LessonsTime>(entity =>
        {
            entity.HasKey(e => e.LessonTimeId).HasName("PK__LessonsT__D0A07B8AD7C46B53");

            entity.ToTable("LessonsTime");

            entity.Property(e => e.LessonTimeId).HasColumnName("LessonTimeID");
        });

        modelBuilder.Entity<StudentsGroup>(entity =>
        {
            entity.HasKey(e => e.StudentsGroupId).HasName("PK__Students__24EA61032F4A1C71");

            entity.Property(e => e.StudentsGroupId).HasColumnName("StudentsGroupID");
            entity.Property(e => e.NumberOfGroup)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Facility).WithMany(p => p.StudentsGroups)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__StudentsG__Facil__6A3BB341");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259445DF2ECF2");

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.TeacherName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TeacherV>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Teacher_v");

            entity.Property(e => e.DisciplineName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DisciplineType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LessonDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

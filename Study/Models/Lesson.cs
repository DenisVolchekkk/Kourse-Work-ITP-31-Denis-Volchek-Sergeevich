using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Study.Models;

public partial class Lesson
{
    [Key]
    [Display(Name = "Код урока")]
    public int LessonId { get; set; }

    [Display(Name = "Код дисциплины")]
    [ForeignKey("Discipline")]
    public int? DisciplineId { get; set; }

    [Display(Name = "Код аудитории")]
    [ForeignKey("Classroom")]
    public int? ClassroomId { get; set; }

    [Display(Name = "Код типа дисциплины")]
    [ForeignKey("DisciplineType")]
    public int? DisciplineTypeId { get; set; }

    [Display(Name = "Код учителя")]
    [ForeignKey("Teacher")]
    public int? TeacherId { get; set; }

    [Display(Name = "Код группы")]
    [ForeignKey("StudentsGroup")]
    public int? StudentsGroupId { get; set; }

    [Display(Name = "Семестр")]
    [Required(ErrorMessage = "Не указан семестр")]
    [Range(1, 2, ErrorMessage = "Семестр может быть только 1 или 2")]
    public int? Semestr { get; set; }
    [Display(Name = "Дата")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Не указана дата")]
    public DateTime? LessonDate { get; set; }

    [Display(Name = "Код времени")]
    [ForeignKey("LessonsTime")]
    public int? LessonTimeId { get; set; }

    [Display(Name = "Год")]
    [Required(ErrorMessage = "Не указан год")]
    public int? Year { get; set; }
    [Display(Name = "День недели")]
    [Required(ErrorMessage = "Не указан день недели")]
    [Range(1, 7, ErrorMessage = "День недели может быть от 1 до 7")]
    public int? DayOfweek { get; set; }

    public virtual Classroom? Classroom { get; set; }

    public virtual Discipline? Discipline { get; set; }

    public virtual DisciplineType? DisciplineType { get; set; }

    public virtual LessonsTime? LessonTime { get; set; }

    public virtual StudentsGroup? StudentsGroup { get; set; }

    public virtual Teacher? Teacher { get; set; }
}

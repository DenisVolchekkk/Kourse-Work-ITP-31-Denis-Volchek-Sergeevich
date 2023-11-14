using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Study.Models;

public partial class Teacher
{
    [Key]
    [Display(Name = "Код учителя")]
    public int TeacherId { get; set; }

    [Display(Name = "Имя учителя")]
    [Required(ErrorMessage = "Не указано имя учителя")]
    public string TeacherName { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

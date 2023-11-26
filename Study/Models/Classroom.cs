using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Study.Models;

public partial class Classroom
{
    [Key]
    [Display(Name = "Код аудитории")]
    public int ClassroomId { get; set; }

    [Display(Name = "Номер аудитории")]
    [Required(ErrorMessage = "Не указан номер аудитории")]
    public int NumberOfClassroom { get; set; }

    [Display(Name = "Кол-во мест")]
    [Required(ErrorMessage = "Не указано количество мест")]
    public int Places { get; set; }

    [Display(Name = "Крыло")]
    [Required(ErrorMessage = "Не указано крыло")]
    public int Wing { get; set; }

    [Display(Name = "Тип аудитории")]
    public string ClassroomType { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

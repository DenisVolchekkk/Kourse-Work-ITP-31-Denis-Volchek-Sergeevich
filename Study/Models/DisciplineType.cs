using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Study.Models;

public partial class DisciplineType
{
    [Key]
    [Display(Name = "Код типа дисциплины")]
    public int DisciplineTypeId { get; set; }

    [Display(Name = "Тип дисциплины")]
    [Required(ErrorMessage = "Не указан тип дисциплины")]
    public string TypeOfDiscipline { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

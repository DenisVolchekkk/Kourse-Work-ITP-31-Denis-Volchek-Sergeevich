using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Study.Models;

public partial class StudentsGroup
{
    [Key]
    [Display(Name = "Код группы")]
    public int StudentsGroupId { get; set; }
    [Display(Name = "Номер группы")]
    [Required(ErrorMessage = "Не указан номер группы")]
    public string NumberOfGroup { get; set; } = null!;
    [Display(Name = "Кол-во студентов")]
    [Required(ErrorMessage = "Не указано кол-во студентов")]
    public int? QuantityOfStudents { get; set; }
    [Display(Name = "Код факультета")]
    [ForeignKey("Facility")]
    public int? FacilityId { get; set; }

    public virtual Facility? Facility { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

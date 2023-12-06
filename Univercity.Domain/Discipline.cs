using System.ComponentModel.DataAnnotations;

namespace Univercity.Domain;

public partial class Discipline
{
    [Key]
    [Display(Name = "Код дисциплины")]
    public int DisciplineId { get; set; }

    [Display(Name = "Дисциплина")]
    [Required(ErrorMessage = "Не указана дисциплина")]
    public string DisciplineName { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

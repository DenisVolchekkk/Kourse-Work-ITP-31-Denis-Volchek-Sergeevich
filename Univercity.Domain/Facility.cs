using System.ComponentModel.DataAnnotations;

namespace Univercity.Domain;

public partial class Facility
{
    [Key]
    [Display(Name = "Код факультета")]
    public int FacilityId { get; set; }

    [Display(Name = "Факультет")]
    [Required(ErrorMessage = "Не указан факультет")]
    public string FacilityName { get; set; } = null!;

    public virtual ICollection<StudentsGroup> StudentsGroups { get; set; } = new List<StudentsGroup>();
}

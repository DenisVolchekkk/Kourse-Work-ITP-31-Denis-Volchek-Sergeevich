using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universaty.Domain
{
    public class StudentsGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentsGroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string NumberOfGroup { get; set; }

        public int QuantityOfStudents { get; set; }

        public int FacilityId { get; set; }

        [ForeignKey("FacilityId")]
        public Facility Facility { get; set; }
    }
}

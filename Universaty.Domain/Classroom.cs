using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universaty.Domain
{
    public class Classroom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassroomID { get; set; }

        public int NumberOfClassroom { get; set; }

        public int Places { get; set; }

        public int Wing { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassroomType { get; set; }
    }
}

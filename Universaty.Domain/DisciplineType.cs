﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universaty.Domain
{
    public class DisciplineTyp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisciplineTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string DisciplineType { get; set; }
    }
}

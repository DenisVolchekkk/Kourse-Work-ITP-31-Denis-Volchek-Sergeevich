using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace Universaty.Domain
{
    public class Teacher
    {

        public int TeacherID { get; set; }

        public string TeacherName { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    }
}

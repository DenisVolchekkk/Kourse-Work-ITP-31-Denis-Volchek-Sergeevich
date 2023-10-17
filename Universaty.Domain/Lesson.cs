using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universaty.Domain
{
    public class Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonID { get; set; }

        public int DisciplineID { get; set; }

        public int ClassroomID { get; set; }

        public int DisciplineTypeID { get; set; }

        public int TeacherID { get; set; }

        public int StudentsGroupID { get; set; }

        public int Semestr { get; set; }

        public DateTime LessonDate { get; set; }

        public int LessonTimeID { get; set; }

        public int Year { get; set; }

        public int DayOfWeek { get; set; }

        [ForeignKey("LessonTimeID")]
        public LessonsTime LessonsTime { get; set; }

        [ForeignKey("ClassroomID")]
        public Classroom Classroom { get; set; }

        [ForeignKey("DisciplineID")]
        public Discipline Discipline { get; set; }

        [ForeignKey("DisciplineTypeID")]
        public DisciplineTyp DisciplineType { get; set; }

        [ForeignKey("TeacherID")]
        public Teacher Teacher { get; set; }

        [ForeignKey("StudentsGroupID")]
        public StudentsGroup StudentsGroup { get; set; }
    }
}

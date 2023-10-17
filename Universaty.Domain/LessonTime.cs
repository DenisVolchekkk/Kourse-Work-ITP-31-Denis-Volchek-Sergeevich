using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universaty.Domain
{

    // Класс модели таблицы LessonsTime
    public class LessonsTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonTimeID { get; set; }

        [Required]
        public TimeSpan LessonTime { get; set; }
    }


}
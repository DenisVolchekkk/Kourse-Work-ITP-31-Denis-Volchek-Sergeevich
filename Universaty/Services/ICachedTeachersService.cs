using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laba3.Models;

namespace Services
{
    public interface ICachedLessonsService
    {
        public IEnumerable<Lesson> GetLessons(int rowsNumber = 20);
        public void AddLessons(string cacheKey, int rowsNumber = 20);
        public IEnumerable<Lesson> GetLessons(string cacheKey, int rowsNumber = 20);
        IEnumerable<Lesson> GetLessonsByTeacherName(string? teacherName, int rowsNumber = 20);
    }
}

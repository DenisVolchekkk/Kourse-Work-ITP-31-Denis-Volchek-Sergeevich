using Microsoft.DotNet.Scaffolding.Shared.CodeModifier;
using Microsoft.Extensions.Caching.Memory;
using Study.Models;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace Study.Services
{
    public class LessonService
    {
        private LessonsDbContext _context;
        private IMemoryCache cache;
        public LessonService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<Lesson> GetLessons()
        {
            cache.TryGetValue("Lessons", out IEnumerable<Lesson>? lessons);

            if (lessons is null)
            {
                lessons = SetLessons();
            }
            return lessons;
        }

        public IEnumerable<Lesson> SetLessons()
        {
            var lessons = _context.Lessons.Include(l => l.LessonTime).Include(l => l.Discipline).Include(l => l.DisciplineType)
                .Include(l => l.StudentsGroup).Include(l => l.Teacher).Include(l => l.Classroom)
                .ToList();
            cache.Set("Lessons", lessons, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return lessons;
        }



    }
}

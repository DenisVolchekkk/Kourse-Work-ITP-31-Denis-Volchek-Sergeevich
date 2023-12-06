using Microsoft.DotNet.Scaffolding.Shared.CodeModifier;
using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace Study.Services
{
    public class LessonService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;
        public LessonService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<Lesson> GetLessons()
        {
            if (!cache.TryGetValue("Lessons", out IEnumerable<Lesson> lessons))
            {
                lessons = SetLessons();
            }
            return lessons;
        }

        public IEnumerable<Lesson> SetLessons()
        {
            var lessons = _context.Lessons
        .ToList();
            foreach (var lesson in lessons)
            {

                _context.Entry(lesson).Reference(l => l.LessonTime).Load();
                _context.Entry(lesson).Reference(l => l.Discipline).Load();
                _context.Entry(lesson).Reference(l => l.DisciplineType).Load();
                _context.Entry(lesson).Reference(l => l.StudentsGroup).Load();
                _context.Entry(lesson.StudentsGroup).Reference(l => l.Facility).Load();
                _context.Entry(lesson).Reference(l => l.Teacher).Load();
                _context.Entry(lesson).Reference(l => l.Classroom).Load();
            }
            cache.Set("Lessons", lessons, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return lessons;
        }



    }
}

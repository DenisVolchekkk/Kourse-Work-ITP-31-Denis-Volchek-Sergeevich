using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class ClassroomService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;

        public ClassroomService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<Classroom> GetClassrooms()
        {
            if (!cache.TryGetValue("Classrooms", out IEnumerable<Classroom> Classrooms))
            {
                Classrooms = SetClassrooms();
            }
            return Classrooms;
        }

        public IEnumerable<Classroom> SetClassrooms()
        {
            var Classrooms = _context.Classrooms
        .ToList();
            foreach (var classroom in Classrooms)
            {

                _context.Entry(classroom).Collection(l => l.Lessons).Load();
                foreach(var lesson in classroom.Lessons)
                {
                    _context.Entry(lesson).Reference(l => l.Classroom).Load();
                    _context.Entry(lesson).Reference(l => l.LessonTime).Load();

                }

            }
            cache.Set("Classrooms", Classrooms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return Classrooms;
        }
    }
}

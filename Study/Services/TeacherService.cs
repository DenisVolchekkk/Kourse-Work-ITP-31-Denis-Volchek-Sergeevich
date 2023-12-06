using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class TeacherService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;
        public TeacherService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<Teacher> GetTeachers()
        {
            if (!cache.TryGetValue("Teachers", out IEnumerable<Teacher> Teachers))
            {
                Teachers = SetTeachers();
            }
            return Teachers;
        }

        public IEnumerable<Teacher> SetTeachers()
        {
            var Teachers = _context.Teachers
        .ToList();
            foreach (var teacher in Teachers)
            {

                _context.Entry(teacher).Collection(l => l.Lessons).Load();


            }
            cache.Set("Teachers", Teachers, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return Teachers;
        }
    }
}

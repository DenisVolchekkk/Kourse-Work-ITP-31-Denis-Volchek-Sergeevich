using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class LessonsTimeService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;

        public LessonsTimeService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<LessonsTime> GetLessonsTimes()
        {
            if (!cache.TryGetValue("LessonsTimes", out IEnumerable<LessonsTime> LessonsTimes))
            {
                LessonsTimes = SetLessonsTimes();
            }
            return LessonsTimes;
        }

        public IEnumerable<LessonsTime> SetLessonsTimes()
        {
            var LessonsTimes = _context.LessonsTimes
        .ToList();
            foreach (var lessonsTime in LessonsTimes)
            {

                _context.Entry(lessonsTime).Collection(l => l.Lessons).Load();


            }
            cache.Set("LessonsTimes", LessonsTimes, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return LessonsTimes;
        }
    }
}

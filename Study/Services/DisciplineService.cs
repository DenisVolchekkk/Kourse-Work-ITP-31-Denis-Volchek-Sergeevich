using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class DisciplineService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;

        public DisciplineService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<Discipline> GetDisciplines()
        {
            if (!cache.TryGetValue("Disciplines", out IEnumerable<Discipline> Disciplines))
            {
                Disciplines = SetDisciplines();
            }
            return Disciplines;
        }

        public IEnumerable<Discipline> SetDisciplines()
        {
            var Disciplines = _context.Disciplines
        .ToList();
            foreach (var discipline in Disciplines)
            {

                _context.Entry(discipline).Collection(l => l.Lessons).Load();


            }
            cache.Set("Disciplines", Disciplines, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return Disciplines;
        }
    }
}

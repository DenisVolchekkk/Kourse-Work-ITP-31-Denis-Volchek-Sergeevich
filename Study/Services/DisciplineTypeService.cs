using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class DisciplineTypeService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;

        public DisciplineTypeService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<DisciplineType> GetDisciplineTypes()
        {
            if (!cache.TryGetValue("DisciplineTypes", out IEnumerable<DisciplineType> DisciplineTypes))
            {
                DisciplineTypes = SetDisciplineTypes();
            }
            return DisciplineTypes;
        }

        public IEnumerable<DisciplineType> SetDisciplineTypes()
        {
            var DisciplineTypes = _context.DisciplineTypes
        .ToList();
            foreach (var disciplineType in DisciplineTypes)
            {

                _context.Entry(disciplineType).Collection(l => l.Lessons).Load();


            }
            cache.Set("DisciplineTypes", DisciplineTypes, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return DisciplineTypes;
        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class StudentsGroupService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;
        public StudentsGroupService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<StudentsGroup> GetStudentsGroups()
        {
            if (!cache.TryGetValue("StudentsGroups", out IEnumerable<StudentsGroup> StudentsGroups))
            {
                StudentsGroups = SetStudentsGroups();
            }
            return StudentsGroups;
        }

        public IEnumerable<StudentsGroup> SetStudentsGroups()
        {
            var StudentsGroups = _context.StudentsGroups
        .ToList();
            foreach (var StudentsGroup in StudentsGroups)
            {

                _context.Entry(StudentsGroup).Reference(l => l.Facility).Load();
                _context.Entry(StudentsGroup).Collection(l => l.Lessons).Load();


            }
            cache.Set("StudentsGroups", StudentsGroups, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return StudentsGroups;
        }
    }
}

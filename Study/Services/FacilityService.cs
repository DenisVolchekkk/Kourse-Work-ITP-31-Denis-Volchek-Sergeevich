using Microsoft.Extensions.Caching.Memory;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Services
{
    public class FacilityService
    {
        private readonly LessonsDbContext _context;
        private readonly IMemoryCache cache;
        
        public FacilityService(LessonsDbContext context, IMemoryCache cache)
        {
            _context = context;
            this.cache = cache;
        }

        public IEnumerable<Facility> GetFacilities()
        {
            if (!cache.TryGetValue("Facilities", out IEnumerable<Facility> Facilities))
            {
                Facilities = SetFacilities();
            }
            return Facilities;
        }

        public IEnumerable<Facility> SetFacilities()
        {
            var Facilities = _context.Facilities
        .ToList();
            foreach (var facility in Facilities)
            {

                _context.Entry(facility).Collection(l => l.StudentsGroups).Load();


            }
            cache.Set("Facilities", Facilities, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(100000)));
            return Facilities;
        }
    }
}

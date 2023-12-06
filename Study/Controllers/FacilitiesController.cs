using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Study.Services;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public FacilitiesController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: Facilities
        // GET: facilityTypes
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();
            FacilitiesViewModel facilitys;
            var facility = HttpContext.Session.Get<FacilitiesViewModel>("Facility");
            if (facility == null)
            {
                facility = new FacilitiesViewModel();
            }
            IEnumerable<Facility> facilityDbContext = cache.GetFacilities();
            facilityDbContext = Sort_Search(facilityDbContext, sortOrder, facility.FacilityName ?? "");
            // Разбиение на страницы
            var count = facilityDbContext.Count();
            facilityDbContext = facilityDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            facilitys = new FacilitiesViewModel
            {
                Facilities = facilityDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FacilityName = facility.FacilityName
            };

            return View(facilitys);
        }
        [HttpPost]
        public IActionResult Index(FacilitiesViewModel facility)
        {

            HttpContext.Session.Set("Facility", facility);

            return RedirectToAction("Index");
        }

        // GET: Facilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();

            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = cache.GetFacilities()
                .FirstOrDefault(m => m.FacilityId == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // GET: Facilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacilityId,FacilityName")] Facility facility)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();

            if (ModelState.IsValid)
            {
                _context.Add(facility);
                await _context.SaveChangesAsync();
                cache.SetFacilities();
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();

            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = cache.GetFacilities()
                .FirstOrDefault(m => m.FacilityId == id);
            if (facility == null)
            {
                return NotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacilityId,FacilityName")] Facility facility)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();
            var cacheSG = HttpContext.RequestServices.GetService<StudentsGroupService>();
            var cacheL = HttpContext.RequestServices.GetService<LessonService>();
            if (id != facility.FacilityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                    cache.SetFacilities();
                    cacheSG.SetStudentsGroups();
                    cacheL.SetLessons();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.FacilityId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();

            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = cache.GetFacilities()
                .FirstOrDefault(m => m.FacilityId == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cache = HttpContext.RequestServices.GetService<FacilityService>();
            var cacheSG = HttpContext.RequestServices.GetService<StudentsGroupService>();
            var cacheL = HttpContext.RequestServices.GetService<LessonService>();
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'LessonsDbContext.Facilities'  is null.");
            }
            var facility = cache.GetFacilities()
.FirstOrDefault(m => m.FacilityId == id);
            var studentGroups =   cacheSG.GetStudentsGroups().Where(c => c.FacilityId == id);
            if (facility != null)
            {
                foreach (var studentGroup in studentGroups)
                {
                    _context.Lessons.RemoveRange(studentGroup.Lessons);
                }
                _context.SaveChanges();
                cache.SetFacilities();
                cacheSG.SetStudentsGroups();
                facility = cache.GetFacilities()
.FirstOrDefault(m => m.FacilityId == id);
                _context.StudentsGroups.RemoveRange(facility.StudentsGroups);
                cache.SetFacilities();
                cacheSG.SetStudentsGroups();
                await _context.SaveChangesAsync();

                _context.Facilities.Remove(facility);
               


            }

            await _context.SaveChangesAsync();
            cache.SetFacilities();
            cacheSG.SetStudentsGroups();
            cacheL.SetLessons();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityExists(int id)
        {
          return (_context.Facilities?.Any(e => e.FacilityId == id)).GetValueOrDefault();
        }
        private IEnumerable<Facility> Sort_Search(IEnumerable<Facility> facilities, SortState sortOrder, string searchFacility)
        {
            switch (sortOrder)
            {
                case SortState.FacilityNameAsc: 
                    facilities = facilities.OrderBy(s => s.FacilityName);
                    break;
                case SortState.FacilityNameDesc:
                    facilities = facilities.OrderByDescending(s => s.FacilityName);
                    break;
            }
            facilities = facilities.Where(o => o.FacilityName.Contains(searchFacility ?? ""));

            return facilities;
        }
    }
}

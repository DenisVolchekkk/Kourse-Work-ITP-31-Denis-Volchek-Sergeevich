using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Univercity.Persistence;
using Study.Services;

namespace Study.Controllers
{
    public class StudentsGroupsController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public StudentsGroupsController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: StudentsGroups
        [ResponseCache(CacheProfileName = "ModelCache")]
        // GET: facilityTypes
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();
            StudentsGroupViewModel studentsGroups;
            var studentsGroup = HttpContext.Session.Get<StudentsGroupViewModel>("StudentGroup");
            if (studentsGroup == null)
            {
                studentsGroup = new StudentsGroupViewModel();
            }
            IEnumerable<StudentsGroup> studentGroupDbContext = cache.GetStudentsGroups();
            studentGroupDbContext = Sort_Search(studentGroupDbContext, sortOrder, studentsGroup.Facility ?? "", studentsGroup.NumberOfGroup ?? "");
            // Разбиение на страницы
            var count = studentGroupDbContext.Count();
            studentGroupDbContext = studentGroupDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            studentsGroups = new StudentsGroupViewModel
            {
                StudentsGroups = studentGroupDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                NumberOfGroup = studentsGroup.NumberOfGroup,
                Facility = studentsGroup.Facility
            };

            return View(studentsGroups);
        }
        [HttpPost]
        public IActionResult Index(StudentsGroupViewModel studentGroup)
        {

            HttpContext.Session.Set("StudentGroup", studentGroup);

            return RedirectToAction("Index");
        }

        // GET: StudentsGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();

            if (id == null || _context.StudentsGroups == null)
            {
                return NotFound();
            }

            var studentsGroup = cache.GetStudentsGroups()
                .FirstOrDefault(m => m.StudentsGroupId == id);
            if (studentsGroup == null)
            {
                return NotFound();
            }

            return View(studentsGroup);
        }

        // GET: StudentsGroups/Create
        public IActionResult Create()
        {
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "FacilityId", "FacilityName");
            return View();
        }

        // POST: StudentsGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentsGroupId,NumberOfGroup,QuantityOfStudents,FacilityId")] StudentsGroup studentsGroup)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();

            if (ModelState.IsValid)
            {
                _context.Add(studentsGroup);
                await _context.SaveChangesAsync();
                cache.SetStudentsGroups();

                return RedirectToAction(nameof(Index));
            }
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "FacilityId", "FacilityName", studentsGroup.FacilityId);
            return View(studentsGroup);
        }

        // GET: StudentsGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();

            if (id == null || _context.StudentsGroups == null)
            {
                return NotFound();
            }

            var studentsGroup = cache.GetStudentsGroups()
                .FirstOrDefault(m => m.StudentsGroupId == id); if (studentsGroup == null)
            {
                return NotFound();
            }
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "FacilityId", "FacilityName", studentsGroup.FacilityId);
            return View(studentsGroup);
        }

        // POST: StudentsGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentsGroupId,NumberOfGroup,QuantityOfStudents,FacilityId")] StudentsGroup studentsGroup)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();

            if (id != studentsGroup.StudentsGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentsGroup);
                    await _context.SaveChangesAsync();
                    cache.SetStudentsGroups();
                    var casheL = HttpContext.RequestServices.GetService<LessonService>();
                    casheL.SetLessons();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsGroupExists(studentsGroup.StudentsGroupId))
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
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "FacilityId", "FacilityName", studentsGroup.FacilityId);
            return View(studentsGroup);
        }

        // GET: StudentsGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();

            if (id == null || _context.StudentsGroups == null)
            {
                return NotFound();
            }

            var studentsGroup = cache.GetStudentsGroups()
                .FirstOrDefault(m => m.StudentsGroupId == id);
            if (studentsGroup == null)
            {
                return NotFound();
            }

            return View(studentsGroup);
        }

        // POST: StudentsGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cache = HttpContext.RequestServices.GetService<StudentsGroupService>();

            if (_context.StudentsGroups == null)
            {
                return Problem("Entity set 'LessonsDbContext.StudentsGroups'  is null.");
            }
            var studentsGroup = cache.GetStudentsGroups()
                .FirstOrDefault(m => m.StudentsGroupId == id);
            if (studentsGroup != null)
            {
                _context.Lessons.RemoveRange(studentsGroup.Lessons);
                _context.StudentsGroups.Remove(studentsGroup);
                

            }

            await _context.SaveChangesAsync();
            cache.SetStudentsGroups();
            var casheL = HttpContext.RequestServices.GetService<LessonService>();
            casheL.SetLessons();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsGroupExists(int id)
        {
          return (_context.StudentsGroups?.Any(e => e.StudentsGroupId == id)).GetValueOrDefault();
        }
        private IEnumerable<StudentsGroup> Sort_Search(IEnumerable<StudentsGroup> studentsGroups, SortState sortOrder, string searchFacility, string searchStudentGroup)
        {
            switch (sortOrder)
            {
                case SortState.StudentGroupAsc:
                    studentsGroups = studentsGroups.OrderBy(s => s.NumberOfGroup);
                    break;
                case SortState.StudentGroupDesc:
                    studentsGroups = studentsGroups.OrderByDescending(s => s.NumberOfGroup);
                    break;
                case SortState.FacilityNameAsc:
                    studentsGroups = studentsGroups.OrderBy(s => s.Facility.FacilityName);
                    break;
                case SortState.FacilityNameDesc:
                    studentsGroups = studentsGroups.OrderByDescending(s => s.Facility.FacilityName);
                    break;
            }
            studentsGroups = studentsGroups
           .Where(o => o.Facility.FacilityName.Contains(searchFacility ?? "") 
           & (o.NumberOfGroup.Contains(searchStudentGroup ?? "")));

            return studentsGroups;
        }
    }
}

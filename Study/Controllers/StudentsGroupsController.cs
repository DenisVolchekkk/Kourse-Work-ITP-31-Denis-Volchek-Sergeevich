using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Study.Filters;
using Study.Models;
using Study.ViewModels;
using System.Drawing.Printing;

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
            StudentsGroupViewModel studentsGroups;
            var studentsGroup = HttpContext.Session.Get<StudentsGroupViewModel>("StudentGroup");
            if (studentsGroup == null)
            {
                studentsGroup = new StudentsGroupViewModel();
            }
            IQueryable<StudentsGroup> studentGroupDbContext = _context.StudentsGroups;
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
            if (id == null || _context.StudentsGroups == null)
            {
                return NotFound();
            }

            var studentsGroup = await _context.StudentsGroups
                .Include(s => s.Facility)
                .FirstOrDefaultAsync(m => m.StudentsGroupId == id);
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
        public async Task<IActionResult> Create([Bind("StudentsGroupId,NumberOfGroup,QuantityOfStudents,FacilityName")] StudentsGroup studentsGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentsGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacilityId"] = new SelectList(_context.Facilities, "FacilityId", "FacilityName", studentsGroup.FacilityId);
            return View(studentsGroup);
        }

        // GET: StudentsGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudentsGroups == null)
            {
                return NotFound();
            }

            var studentsGroup = await _context.StudentsGroups.FindAsync(id);
            if (studentsGroup == null)
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
            if (id == null || _context.StudentsGroups == null)
            {
                return NotFound();
            }

            var studentsGroup = await _context.StudentsGroups
                .Include(s => s.Facility)
                .FirstOrDefaultAsync(m => m.StudentsGroupId == id);
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
            if (_context.StudentsGroups == null)
            {
                return Problem("Entity set 'LessonsDbContext.StudentsGroups'  is null.");
            }
            var studentsGroup = await _context.StudentsGroups.Include(c => c.Lessons).FirstOrDefaultAsync(c => c.StudentsGroupId == id);
            if (studentsGroup != null)
            {
                _context.StudentsGroups.Remove(studentsGroup);
                _context.Lessons.RemoveRange(studentsGroup.Lessons);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsGroupExists(int id)
        {
          return (_context.StudentsGroups?.Any(e => e.StudentsGroupId == id)).GetValueOrDefault();
        }
        private IQueryable<StudentsGroup> Sort_Search(IQueryable<StudentsGroup> studentsGroups, SortState sortOrder, string searchFacility, string searchStudentGroup)
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
            studentsGroups = studentsGroups.Include(l => l.Facility)
           .Where(o => o.Facility.FacilityName.Contains(searchFacility ?? "") 
           & (o.NumberOfGroup.Contains(searchStudentGroup ?? "")));

            return studentsGroups;
        }
    }
}

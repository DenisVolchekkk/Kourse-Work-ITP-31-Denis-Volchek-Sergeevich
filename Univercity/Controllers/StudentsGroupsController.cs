using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Univercity.Models;

namespace Univercity.Controllers
{
    public class StudentsGroupsController : Controller
    {
        private readonly LessonsDbContext _context;

        public StudentsGroupsController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: StudentsGroups
        [ResponseCache(CacheProfileName = "ModelCache")]
        public async Task<IActionResult> Index()
        {
            var lessonsDbContext = _context.StudentsGroups.Include(s => s.Facility);
            return View(await lessonsDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("StudentsGroupId,NumberOfGroup,QuantityOfStudents,FacilityId")] StudentsGroup studentsGroup)
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
            var studentsGroup = await _context.StudentsGroups.FindAsync(id);
            if (studentsGroup != null)
            {
                _context.StudentsGroups.Remove(studentsGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsGroupExists(int id)
        {
          return (_context.StudentsGroups?.Any(e => e.StudentsGroupId == id)).GetValueOrDefault();
        }
    }
}

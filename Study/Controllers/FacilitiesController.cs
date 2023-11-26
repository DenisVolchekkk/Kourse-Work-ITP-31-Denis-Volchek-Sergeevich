using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Study.Filters;
using Study.Models;
using Study.ViewModels;

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
            FacilitiesViewModel facilitys;
            var facility = HttpContext.Session.Get<FacilitiesViewModel>("Facility");
            if (facility == null)
            {
                facility = new FacilitiesViewModel();
            }
            IQueryable<Facility> facilityDbContext = _context.Facilities;
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
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .FirstOrDefaultAsync(m => m.FacilityId == id);
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
            if (ModelState.IsValid)
            {
                _context.Add(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities.FindAsync(id);
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
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .FirstOrDefaultAsync(m => m.FacilityId == id);
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
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'LessonsDbContext.Facilities'  is null.");
            }
            var facility = await _context.Facilities.Include(c => c.StudentsGroups).FirstOrDefaultAsync(c => c.FacilityId == id);
            var studentGroups =   _context.StudentsGroups.Include(c => c.Lessons).Where(c => c.FacilityId == id).ToList();
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                _context.StudentsGroups.RemoveRange(facility.StudentsGroups);
                foreach(var studentGroup in studentGroups)
                {
                    _context.Lessons.RemoveRange(studentGroup.Lessons);
                }

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityExists(int id)
        {
          return (_context.Facilities?.Any(e => e.FacilityId == id)).GetValueOrDefault();
        }
        private IQueryable<Facility> Sort_Search(IQueryable<Facility> facilities, SortState sortOrder, string searchFacility)
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

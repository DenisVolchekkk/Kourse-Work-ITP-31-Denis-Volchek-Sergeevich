using System;
using System.Collections.Generic;
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
    public class LessonsTimesController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public LessonsTimesController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: LessonsTimes
        // GET: DisciplineTypes
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            LessonTimesViewModel lessonTimes;
            var lessonTime = HttpContext.Session.Get<LessonTimesViewModel>("LessonTime");
            if (lessonTime == null)
            {
                lessonTime = new LessonTimesViewModel();
            }
            IQueryable<LessonsTime> lessonTimeDbContext = _context.LessonsTimes;
            lessonTimeDbContext = Sort_Search(lessonTimeDbContext, sortOrder, lessonTime.LessonTime);
            // Разбиение на страницы
            var count = lessonTimeDbContext.Count();
            lessonTimeDbContext = lessonTimeDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            lessonTimes = new LessonTimesViewModel
            {
                LessonsTimes = lessonTimeDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                LessonTime = lessonTime.LessonTime
            };

            return View(lessonTimes);
        }
        [HttpPost]
        public IActionResult Index(LessonTimesViewModel lessonsTime)
        {

            HttpContext.Session.Set("LessonTime", lessonsTime);

            return RedirectToAction("Index");
        }

        // GET: LessonsTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LessonsTimes == null)
            {
                return NotFound();
            }

            var lessonsTime = await _context.LessonsTimes
                .FirstOrDefaultAsync(m => m.LessonTimeId == id);
            if (lessonsTime == null)
            {
                return NotFound();
            }

            return View(lessonsTime);
        }

        // GET: LessonsTimes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LessonsTimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonTimeId,LessonTime")] LessonsTime lessonsTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lessonsTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lessonsTime);
        }

        // GET: LessonsTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LessonsTimes == null)
            {
                return NotFound();
            }

            var lessonsTime = await _context.LessonsTimes.FindAsync(id);
            if (lessonsTime == null)
            {
                return NotFound();
            }
            return View(lessonsTime);
        }

        // POST: LessonsTimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonTimeId,LessonTime")] LessonsTime lessonsTime)
        {
            if (id != lessonsTime.LessonTimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessonsTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonsTimeExists(lessonsTime.LessonTimeId))
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
            return View(lessonsTime);
        }

        // GET: LessonsTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LessonsTimes == null)
            {
                return NotFound();
            }

            var lessonsTime = await _context.LessonsTimes
                .FirstOrDefaultAsync(m => m.LessonTimeId == id);
            if (lessonsTime == null)
            {
                return NotFound();
            }

            return View(lessonsTime);
        }

        // POST: LessonsTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LessonsTimes == null)
            {
                return Problem("Entity set 'LessonsDbContext.LessonsTimes'  is null.");
            }
            var lessonsTime = await _context.LessonsTimes.FindAsync(id);
            if (lessonsTime != null)
            {
                _context.LessonsTimes.Remove(lessonsTime);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonsTimeExists(int id)
        {
          return (_context.LessonsTimes?.Any(e => e.LessonTimeId == id)).GetValueOrDefault();
        }
        private IQueryable<LessonsTime> Sort_Search(IQueryable<LessonsTime> lessonsTimes, SortState sortOrder, TimeSpan searchLessonTime)
        {
            switch (sortOrder)
            {
                case SortState.DisciplineNameAsc:
                    lessonsTimes = lessonsTimes.OrderBy(s => s.LessonTime);
                    break;
                case SortState.DisciplineNameDesc:
                    lessonsTimes = lessonsTimes.OrderByDescending(s => s.LessonTime);
                    break;
            }
            lessonsTimes = lessonsTimes.Where(o => o.LessonTime == searchLessonTime || searchLessonTime == new TimeSpan());

            return lessonsTimes;
        }
    }
}

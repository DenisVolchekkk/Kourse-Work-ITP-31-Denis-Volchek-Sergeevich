using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Study.Services;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Univercity.Persistence;

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
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

            LessonTimesViewModel lessonTimes;
            var lessonTime = HttpContext.Session.Get<LessonTimesViewModel>("LessonTime");
            if (lessonTime == null)
            {
                lessonTime = new LessonTimesViewModel();
            }
            IEnumerable<LessonsTime> lessonTimeDbContext = cache.GetLessonsTimes();
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
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

            if (id == null || _context.LessonsTimes == null)
            {
                return NotFound();
            }

            var lessonsTime = cache.GetLessonsTimes()
                .FirstOrDefault(m => m.LessonTimeId == id);
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
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

            if (ModelState.IsValid)
            {
                _context.Add(lessonsTime);
                await _context.SaveChangesAsync();
                cache.SetLessonsTimes();

                return RedirectToAction(nameof(Index));
            }
            return View(lessonsTime);
        }

        // GET: LessonsTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

            if (id == null || _context.LessonsTimes == null)
            {
                return NotFound();
            }

            var lessonsTime = cache.GetLessonsTimes()
                 .FirstOrDefault(m => m.LessonTimeId == id); if (lessonsTime == null)
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
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

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
                    cache.SetLessonsTimes();
                    var casheL = HttpContext.RequestServices.GetService<LessonService>();
                    casheL.SetLessons();

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
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

            if (id == null || _context.LessonsTimes == null)
            {
                return NotFound();
            }

            var lessonsTime = cache.GetLessonsTimes()
                 .FirstOrDefault(m => m.LessonTimeId == id);
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
            var cache = HttpContext.RequestServices.GetService<LessonsTimeService>();

            if (_context.LessonsTimes == null)
            {
                return Problem("Entity set 'LessonsDbContext.LessonsTimes'  is null.");
            }
            var lessonsTime = cache.GetLessonsTimes()
                 .FirstOrDefault(m => m.LessonTimeId == id);
            if (lessonsTime != null)
            {
                _context.LessonsTimes.Remove(lessonsTime);
                _context.Lessons.RemoveRange(lessonsTime.Lessons);

            }

            await _context.SaveChangesAsync();
            cache.SetLessonsTimes();
            var casheL = HttpContext.RequestServices.GetService<LessonService>();
            casheL.SetLessons();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonsTimeExists(int id)
        {
          return (_context.LessonsTimes?.Any(e => e.LessonTimeId == id)).GetValueOrDefault();
        }
        private IEnumerable<LessonsTime> Sort_Search(IEnumerable<LessonsTime> lessonsTimes, SortState sortOrder, TimeSpan searchLessonTime)
        {
            switch (sortOrder)
            {
                case SortState.LessonTimeAsc:
                    lessonsTimes = lessonsTimes.OrderBy(s => s.LessonTime);
                    break;
                case SortState.LessonTimeDesc:
                    lessonsTimes = lessonsTimes.OrderByDescending(s => s.LessonTime);
                    break;
            }
            lessonsTimes = lessonsTimes.Where(o => o.LessonTime == searchLessonTime || searchLessonTime == new TimeSpan());

            return lessonsTimes;
        }
    }
}

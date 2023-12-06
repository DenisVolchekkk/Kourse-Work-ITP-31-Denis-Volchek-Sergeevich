using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Study.Services;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Controllers
{
    public class DisciplinesController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public DisciplinesController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: DisciplineTypes
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            DisciplinesViewModel disciplines;
            var discipline = HttpContext.Session.Get<DisciplinesViewModel>("Discipline");
            if (discipline == null)
            {
                discipline = new DisciplinesViewModel();
            }
            IEnumerable<Discipline> disciplineDbContext = cache.GetDisciplines();
            disciplineDbContext = Sort_Search(disciplineDbContext, sortOrder, discipline.DisciplineName ?? "");
            // Разбиение на страницы
            var count = disciplineDbContext.Count();
            disciplineDbContext = disciplineDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            disciplines = new DisciplinesViewModel
            {
                Disciplines = disciplineDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                DisciplineName = discipline.DisciplineName
            };

            return View(disciplines);
        }
        [HttpPost]
        public IActionResult Index(DisciplinesViewModel discipline)
        {

            HttpContext.Session.Set("Discipline", discipline);

            return RedirectToAction("Index");
        }
        // GET: Disciplines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            if (id == null || _context.Disciplines == null)
            {
                return NotFound();
            }

            var discipline = cache.GetDisciplines()
                .FirstOrDefault(m => m.DisciplineId == id);
            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        // GET: Disciplines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisciplineId,DisciplineName")] Discipline discipline)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            if (ModelState.IsValid)
            {
                _context.Add(discipline);
                await _context.SaveChangesAsync();
                cache.SetDisciplines();

                return RedirectToAction(nameof(Index));
            }
            return View(discipline);
        }

        // GET: Disciplines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            if (id == null || _context.Disciplines == null)
            {
                return NotFound();
            }

            var discipline = cache.GetDisciplines()
                .FirstOrDefault(m => m.DisciplineId == id);
            if (discipline == null)
            {
                return NotFound();
            }
            return View(discipline);
        }

        // POST: Disciplines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisciplineId,DisciplineName")] Discipline discipline)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            if (id != discipline.DisciplineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discipline);
                    await _context.SaveChangesAsync();
                    cache.SetDisciplines();
                    var casheL = HttpContext.RequestServices.GetService<LessonService>();
                    casheL.SetLessons();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplineExists(discipline.DisciplineId))
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
            return View(discipline);
        }

        // GET: Disciplines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            if (id == null || _context.Disciplines == null)
            {
                return NotFound();
            }
            var discipline = cache.GetDisciplines()
                .FirstOrDefault(m => m.DisciplineId == id);
            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineService>();

            if (_context.Disciplines == null)
            {
                return Problem("Entity set 'LessonsDbContext.Disciplines'  is null.");
            }
            var discipline = cache.GetDisciplines()
                .FirstOrDefault(m => m.DisciplineId == id); 
            if (discipline != null)
            {
                _context.Disciplines.Remove(discipline);
                _context.Lessons.RemoveRange(discipline.Lessons);
            }
            
            await _context.SaveChangesAsync();
            cache.SetDisciplines();
            var casheL = HttpContext.RequestServices.GetService<LessonService>();
            casheL.SetLessons();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplineExists(int id)
        {
          return (_context.Disciplines?.Any(e => e.DisciplineId == id)).GetValueOrDefault();
        }
        private IEnumerable<Discipline> Sort_Search(IEnumerable<Discipline> disciplines, SortState sortOrder, string searchDiscipline)
        {
            switch (sortOrder)
            {
                case SortState.DisciplineNameAsc:
                    disciplines = disciplines.OrderBy(s => s.DisciplineName);
                    break;
                case SortState.DisciplineNameDesc:
                    disciplines = disciplines.OrderByDescending(s => s.DisciplineName);
                    break;
            }
            disciplines = disciplines.Where(o => o.DisciplineName.Contains(searchDiscipline ?? ""));

            return disciplines;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Study.Services;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Controllers
{
    public class DisciplineTypesController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public DisciplineTypesController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: DisciplineTypes
        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            DisciplineTypeViewModel disciplines;
            var disciplineType = HttpContext.Session.Get<DisciplineTypeViewModel>("DisciplineType");
            if (disciplineType == null)
            {
                disciplineType = new DisciplineTypeViewModel();
            }
            IEnumerable<DisciplineType> disciplineTypeDbContext = cache.GetDisciplineTypes();
            disciplineTypeDbContext = Sort_Search(disciplineTypeDbContext, sortOrder, disciplineType.TypeOfDiscipline ?? "");
            // Разбиение на страницы
            var count = disciplineTypeDbContext.Count();
            disciplineTypeDbContext = disciplineTypeDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            disciplines = new DisciplineTypeViewModel
            {
                DisciplineTypes = disciplineTypeDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                TypeOfDiscipline = disciplineType.TypeOfDiscipline
            };

            return View(disciplines);
        }
        [HttpPost]
        public IActionResult Index(DisciplineTypeViewModel disciplineType)
        {

            HttpContext.Session.Set("DisciplineType", disciplineType);

            return RedirectToAction("Index");
        }

        // GET: DisciplineTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            if (id == null || _context.DisciplineTypes == null)
            {
                return NotFound();
            }

            var disciplineType = cache.GetDisciplineTypes()
                .FirstOrDefault(m => m.DisciplineTypeId == id);
            if (disciplineType == null)
            {
                return NotFound();
            }

            return View(disciplineType);
        }

        // GET: DisciplineTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DisciplineTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisciplineTypeId,TypeOfDiscipline")] DisciplineType disciplineType)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            if (ModelState.IsValid)
            {
                _context.Add(disciplineType);
                await _context.SaveChangesAsync();
                cache.SetDisciplineTypes();

                return RedirectToAction(nameof(Index));
            }
            return View(disciplineType);
        }

        // GET: DisciplineTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            if (id == null || _context.DisciplineTypes == null)
            {
                return NotFound();
            }


            var disciplineType = cache.GetDisciplineTypes()
                .FirstOrDefault(m => m.DisciplineTypeId == id); if (disciplineType == null)
            {
                return NotFound();
            }
            return View(disciplineType);
        }

        // POST: DisciplineTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DisciplineTypeId,TypeOfDiscipline")] DisciplineType disciplineType)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            if (id != disciplineType.DisciplineTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplineType);
                    await _context.SaveChangesAsync();
                    cache.SetDisciplineTypes();
                    var casheL = HttpContext.RequestServices.GetService<LessonService>();
                    casheL.SetLessons();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplineTypeExists(disciplineType.DisciplineTypeId))
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
            return View(disciplineType);
        }

        // GET: DisciplineTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            if (id == null || _context.DisciplineTypes == null)
            {
                return NotFound();
            }


            var disciplineType = cache.GetDisciplineTypes()
                .FirstOrDefault(m => m.DisciplineTypeId == id);
            if (disciplineType == null)
            {
                return NotFound();
            }

            return View(disciplineType);
        }

        // POST: DisciplineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cache = HttpContext.RequestServices.GetService<DisciplineTypeService>();

            if (_context.DisciplineTypes == null)
            {
                return Problem("Entity set 'LessonsDbContext.DisciplineTypes'  is null.");
            }

            var disciplineType = cache.GetDisciplineTypes()
                .FirstOrDefault(m => m.DisciplineTypeId == id); if (disciplineType != null)
            {
                _context.DisciplineTypes.Remove(disciplineType);
                _context.Lessons.RemoveRange(disciplineType.Lessons);

            }

            await _context.SaveChangesAsync();
            cache.SetDisciplineTypes();
            var casheL = HttpContext.RequestServices.GetService<LessonService>();
            casheL.SetLessons();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplineTypeExists(int id)
        {
          return (_context.DisciplineTypes?.Any(e => e.DisciplineTypeId == id)).GetValueOrDefault();
        }
        private IEnumerable<DisciplineType> Sort_Search(IEnumerable<DisciplineType> disciplineTypes, SortState sortOrder, string searchDisciplineType)
        {
            switch (sortOrder)
            {
                case SortState.DisciplineTypeAsc:
                    disciplineTypes = disciplineTypes.OrderBy(s => s.TypeOfDiscipline);
                    break;
                case SortState.DisciplineTypeDesc:
                    disciplineTypes = disciplineTypes.OrderByDescending(s => s.TypeOfDiscipline);
                    break;
            }
            disciplineTypes = disciplineTypes.Where(o => o.TypeOfDiscipline.Contains(searchDisciplineType ?? ""));

            return disciplineTypes;
        }
    }
}

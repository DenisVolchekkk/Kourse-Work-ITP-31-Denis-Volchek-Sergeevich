using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Study.Services;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Univercity.Persistence;

namespace Study.Controllers
{
    public class TeachersController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public TeachersController(LessonsDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(SortState sortOrder, int page = 1)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            TeachersViewModel teachers;
            var teacher = HttpContext.Session.Get<TeachersViewModel>("Teacher");
            if (teacher == null)
            {
                teacher = new TeachersViewModel();
            }
            IEnumerable<Teacher> teacherDbContext = cache.GetTeachers();
            teacherDbContext = Sort_Search(teacherDbContext, sortOrder, teacher.TeacherName ?? "");
            // Разбиение на страницы
            var count = teacherDbContext.Count();
            teacherDbContext = teacherDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            teachers = new TeachersViewModel
            {
                Teachers = teacherDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                TeacherName = teacher.TeacherName
            };

            return View(teachers);
        }
        [HttpPost]
        public IActionResult Index(TeachersViewModel teacher)
        {

            HttpContext.Session.Set("Teacher", teacher);

            return RedirectToAction("Index");
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = cache.GetTeachers()
                .FirstOrDefault(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,TeacherName")] Teacher teacher)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                cache.SetTeachers();

                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = cache.GetTeachers()
                .FirstOrDefault(m => m.TeacherId == id); if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,TeacherName")] Teacher teacher)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                    cache.SetTeachers();
                    var casheL = HttpContext.RequestServices.GetService<LessonService>();
                    casheL.SetLessons();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = cache.GetTeachers()
                .FirstOrDefault(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cache = HttpContext.RequestServices.GetService<TeacherService>();

            if (_context.Teachers == null)
            {
                return Problem("Entity set 'LessonsDbContext.Teachers'  is null.");
            }
            var teacher = cache.GetTeachers()
                .FirstOrDefault(m => m.TeacherId == id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.Lessons.RemoveRange(teacher.Lessons);

            }

            await _context.SaveChangesAsync();
            cache.SetTeachers();
            var casheL = HttpContext.RequestServices.GetService<LessonService>();
            casheL.SetLessons();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
          return (_context.Teachers?.Any(e => e.TeacherId == id)).GetValueOrDefault();
        }
        private IEnumerable<Teacher> Sort_Search(IEnumerable<Teacher> teachers, SortState sortOrder, string searchTeacher)
        {
            switch (sortOrder)
            {
                case SortState.TeacherAsc:
                    teachers = teachers.OrderBy(s => s.TeacherName);
                    break;
                case SortState.TeacherDesc:
                    teachers = teachers.OrderByDescending(s => s.TeacherName);
                    break;
            }
            teachers = teachers.Where(o => o.TeacherName.Contains(searchTeacher ?? ""));

            return teachers;
        }
    }
}

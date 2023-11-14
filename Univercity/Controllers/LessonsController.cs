using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Univercity.Models;
using Microsoft.Data.SqlClient;
using Univercity.ViewModels;

namespace Univercity.Controllers
{
    public class LessonsController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12
            ;   // количество элементов на странице
        public LessonsController(LessonsDbContext context)
        {
            _context = context;

        }

        // GET: Lessons
        [ResponseCache(CacheProfileName = "ModelCache")]
        public async Task<IActionResult> Index(int page = 1)
        {

            IQueryable<Lesson> lessonsDbContext = _context.Lessons.Include(l => l.Classroom).Include(l => l.Discipline).Include(l => l.DisciplineType).Include(l => l.LessonTime).Include(l => l.StudentsGroup).Include(l => l.Teacher);
            // Разбиение на страницы
            var count = lessonsDbContext.Count();
            lessonsDbContext = lessonsDbContext.Skip((page - 1) * pageSize).Take(pageSize);
            LessonViewModel lessons = new LessonViewModel
            {
                Lessons = lessonsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
            };
            return View(lessons);
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Discipline)
                .Include(l => l.DisciplineType)
                .Include(l => l.LessonTime)
                .Include(l => l.StudentsGroup)
                .Include(l => l.Teacher)
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // GET: Lessons/Create
        public IActionResult Create()
        {
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "NumberOfClassroom");
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineName");
            ViewData["DisciplineTypeId"] = new SelectList(_context.DisciplineTypes, "DisciplineTypeId", "TypeOfDiscipline");
            ViewData["LessonTimeId"] = new SelectList(_context.LessonsTimes, "LessonTimeId", "LessonTime");
            ViewData["StudentsGroupId"] = new SelectList(_context.StudentsGroups, "StudentsGroupId", "NumberOfGroup");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LessonId,DisciplineId,ClassroomId,DisciplineTypeId,TeacherId,StudentsGroupId,Semestr,LessonDate,LessonTimeId,Year,DayOfweek")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "NumberOfClassroom", lesson.ClassroomId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineName", lesson.DisciplineId);
            ViewData["DisciplineTypeId"] = new SelectList(_context.DisciplineTypes, "DisciplineTypeId", "TypeOfDiscipline", lesson.DisciplineTypeId);
            ViewData["LessonTimeId"] = new SelectList(_context.LessonsTimes, "LessonTimeId", "LessonTime", lesson.LessonTimeId);
            ViewData["StudentsGroupId"] = new SelectList(_context.StudentsGroups, "StudentsGroupId", "NumberOfGroup", lesson.StudentsGroupId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherName", lesson.TeacherId);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "NumberOfClassroom", lesson.ClassroomId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineName", lesson.DisciplineId);
            ViewData["DisciplineTypeId"] = new SelectList(_context.DisciplineTypes, "DisciplineTypeId", "TypeOfDiscipline", lesson.DisciplineTypeId);
            ViewData["LessonTimeId"] = new SelectList(_context.LessonsTimes, "LessonTimeId", "LessonTime", lesson.LessonTimeId);
            ViewData["StudentsGroupId"] = new SelectList(_context.StudentsGroups, "StudentsGroupId", "NumberOfGroup", lesson.StudentsGroupId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherName", lesson.TeacherId);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LessonId,DisciplineId,ClassroomId,DisciplineTypeId,TeacherId,StudentsGroupId,Semestr,LessonDate,LessonTimeId,Year,DayOfweek")] Lesson lesson)
        {
            if (id != lesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(lesson.LessonId))
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
            ViewData["ClassroomId"] = new SelectList(_context.Classrooms, "ClassroomId", "NumberOfClassroom", lesson.ClassroomId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "DisciplineId", "DisciplineName", lesson.DisciplineId);
            ViewData["DisciplineTypeId"] = new SelectList(_context.DisciplineTypes, "DisciplineTypeId", "TypeOfDiscipline", lesson.DisciplineTypeId);
            ViewData["LessonTimeId"] = new SelectList(_context.LessonsTimes, "LessonTimeId", "LessonTime", lesson.LessonTimeId);
            ViewData["StudentsGroupId"] = new SelectList(_context.StudentsGroups, "StudentsGroupId", "NumberOfGroup", lesson.StudentsGroupId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "TeacherName", lesson.TeacherId);
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Classroom)
                .Include(l => l.Discipline)
                .Include(l => l.DisciplineType)
                .Include(l => l.LessonTime)
                .Include(l => l.StudentsGroup)
                .Include(l => l.Teacher)
                .FirstOrDefaultAsync(m => m.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'LessonsDbContext.Lessons'  is null.");
            }
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
          return (_context.Lessons?.Any(e => e.LessonId == id)).GetValueOrDefault();
        }
    }
}

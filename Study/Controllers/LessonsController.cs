using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Study.Extensions;
using Univercity.Application.ViewModels;
using Univercity.Domain;
using Study.Services;
using Univercity.Persistence;

namespace Study.Controllers
{
    public class LessonsController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 8;  // количество элементов на странице
        public LessonsController(LessonsDbContext context)
        {
            _context = context;

        }
        // GET: Lessons
        [ResponseCache(CacheProfileName = "ModelCache")]
        //[SetToSession("Less")]
        public IActionResult Index(SortState sortOrder, int page = 1)
        {
            var cache = HttpContext.RequestServices.GetService<LessonService>();

            LessonsViewModel lessons;
            var lesson = HttpContext.Session.Get<FilterLessonViewModel>("Lesson");
            if (lesson == null)
            {
                lesson = new FilterLessonViewModel();
            }
            IEnumerable<Lesson> lessonsDbContext = cache.GetLessons();
            lessonsDbContext = Sort_Search(lessonsDbContext, sortOrder, lesson.Discipline ?? "", lesson.Teacher ?? "", lesson.LessonTime, lesson.Facility ?? "", lesson.Classroom,
                lesson.StudentGroup ?? "", lesson.DisciplineType ?? "", lesson.Year, lesson.DayOfweek, lesson.Semestr, lesson.LessonDate);
            // Разбиение на страницы

            var count = lessonsDbContext.Count();
            lessonsDbContext = lessonsDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            lessons = new LessonsViewModel
            {
                Lessons = lessonsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterLessonViewModel = lesson
            };
            return View(lessons);
        }
        [HttpPost]
        public IActionResult Index(FilterLessonViewModel lesson)
        {

            HttpContext.Session.Set("Lesson", lesson);

            return RedirectToAction("Index");
        }


        // GET: Lessons/Details/5
        public IActionResult Details(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<LessonService>();
            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = _context.Lessons.Include(l => l.Classroom).Include(l => l.Discipline).Include(l => l.DisciplineType).Include(l => l.LessonTime).Include(l => l.StudentsGroup).Include(l => l.Teacher)
                .FirstOrDefault(m => m.LessonId == id);
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
            var cache = HttpContext.RequestServices.GetService<LessonService>();

            if (ModelState.IsValid)
            {
                lesson.Year = lesson.LessonDate.Value.Year;
                lesson.DayOfweek = ((int)lesson.LessonDate.Value.DayOfWeek + 6) % 7 + 1;
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                cache.SetLessons();
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
        public IActionResult Edit(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<LessonService>();

            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = cache.GetLessons().FirstOrDefault(m => m.LessonId == id);
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
            var cache = HttpContext.RequestServices.GetService<LessonService>();

            if (id != lesson.LessonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    lesson.Year = lesson.LessonDate.Value.Year;
                    lesson.DayOfweek = ((int)lesson.LessonDate.Value.DayOfWeek + 6) % 7 + 1;
                    _context.Update(lesson);
                    await _context.SaveChangesAsync();
                    cache.SetLessons();

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
        public IActionResult Delete(int? id)
        {
            var cache = HttpContext.RequestServices.GetService<LessonService>();

            if (id == null || _context.Lessons == null)
            {
                return NotFound();
            }

            var lesson = cache.GetLessons().FirstOrDefault(l => l.LessonId == id);
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
            var cache = HttpContext.RequestServices.GetService<LessonService>();

            if (_context.Lessons == null)
            {
                return Problem("Entity set 'LessonsDbContext.Lessons'  is null.");
            }
            var lesson = cache.GetLessons().FirstOrDefault(l => l.LessonId == id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                cache.SetLessons();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.LessonId == id);
        }
        private IEnumerable<Lesson> Sort_Search(IEnumerable<Lesson> lessons, SortState sortOrder, string searchDisciplineName, string searchTeacher, TimeSpan searchLessonTime,
            string searchFacility, int searchClassroom, string searchStudentGroup, string searchDisciplineType, int searchYear, int searchDayofWeek, int searchSemestr, DateTime? searchDate)
        {
            switch (sortOrder)
            {
                case SortState.DisciplineNameAsc:
                    lessons = lessons.OrderBy(s => s.Discipline.DisciplineName);
                    break;
                case SortState.DisciplineNameDesc:
                    lessons = lessons.OrderByDescending(s => s.Discipline.DisciplineName);
                    break;
                case SortState.TeacherAsc:
                    lessons = lessons.OrderBy(s => s.Teacher.TeacherName);
                    break;
                case SortState.TeacherDesc:
                    lessons = lessons.OrderByDescending(s => s.Teacher.TeacherName);
                    break;
                case SortState.LessonTimeAsc:
                    lessons = lessons.OrderBy(s => s.LessonTime.LessonTime);
                    break;
                case SortState.LessonTimeDesc:
                    lessons = lessons.OrderByDescending(s => s.LessonTime.LessonTime);
                    break;
                case SortState.ClassroomAsc:
                    lessons = lessons.OrderBy(s => s.Classroom.NumberOfClassroom);
                    break;
                case SortState.ClassroomDesc:
                    lessons = lessons.OrderByDescending(s => s.Classroom.NumberOfClassroom);
                    break;
                case SortState.StudentGroupAsc:
                    lessons = lessons.OrderBy(s => s.StudentsGroup.NumberOfGroup);
                    break;
                case SortState.StudentGroupDesc:
                    lessons = lessons.OrderByDescending(s => s.StudentsGroup.NumberOfGroup);
                    break;
                case SortState.DisciplineTypeAsc:
                    lessons = lessons.OrderBy(s => s.DisciplineType.TypeOfDiscipline);
                    break;
                case SortState.DisciplineTypeDesc:
                    lessons = lessons.OrderByDescending(s => s.DisciplineType.TypeOfDiscipline);
                    break;
                case SortState.YearAsc:
                    lessons = lessons.OrderBy(s => s.Year);
                    break;
                case SortState.YearDesc:
                    lessons = lessons.OrderByDescending(s => s.Year);
                    break;
                case SortState.DayofWeekAsc:
                    lessons = lessons.OrderBy(s => s.DayOfweek);
                    break;
                case SortState.DayofWeekDesc:
                    lessons = lessons.OrderByDescending(s => s.DayOfweek);
                    break;
                case SortState.SemestrAsc:
                    lessons = lessons.OrderBy(s => s.Semestr);
                    break;
                case SortState.SemestrDesc:
                    lessons = lessons.OrderByDescending(s => s.Semestr);
                    break;
                case SortState.DateAsc:
                    lessons = lessons.OrderBy(s => s.LessonDate);
                    break;
                case SortState.DateDesc:
                    lessons = lessons.OrderByDescending(s => s.LessonDate);
                    break;
            }
            lessons = lessons
           .Where(o => o.Discipline.DisciplineName.Contains(searchDisciplineName ?? "")
           & o.Teacher.TeacherName.Contains(searchTeacher ?? "")
           & (o.Classroom.NumberOfClassroom == searchClassroom || searchClassroom == 0)
           & (o.Year == searchYear || searchYear == 0)
           & (o.DayOfweek == searchDayofWeek || searchDayofWeek == 0)
           & (o.Semestr == searchSemestr || searchSemestr == 0)
           & (o.StudentsGroup.Facility.FacilityName.Contains(searchFacility ?? ""))
           & (o.StudentsGroup.NumberOfGroup.Contains(searchStudentGroup ?? ""))
           & (o.DisciplineType.TypeOfDiscipline.Contains(searchDisciplineType ?? ""))
           & (o.LessonDate == searchDate || searchDate == new DateTime() || searchDate == null)
           & (o.LessonTime.LessonTime == searchLessonTime || searchLessonTime == new TimeSpan()));

            return lessons;
        }
    }

}
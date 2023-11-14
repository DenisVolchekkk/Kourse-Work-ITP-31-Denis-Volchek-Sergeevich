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
    public class ClassroomsController : Controller
    {
        private readonly LessonsDbContext _context;
        private readonly int pageSize = 12;

        public ClassroomsController(LessonsDbContext context)
        {
            _context = context;
        }

        // GET: Classrooms
        [ResponseCache(CacheProfileName = "ModelCache")]
        //[SetToSession("Less")]
        public IActionResult Index(SortState sortOrder, int page = 1)
        {
            ClassroomsViewModel classrooms;
            var classroom = HttpContext.Session.Get<ClassroomsViewModel>("Classroom");
            if (classroom == null)
            {
                classroom = new ClassroomsViewModel();
                
            }
            IQueryable<Classroom> classroomsDbContext = _context.Classrooms;
            classroomsDbContext = Sort_Search(classroomsDbContext, sortOrder, classroom.NumberOfClassroom);
            // Разбиение на страницы
            var count = classroomsDbContext.Count();
            classroomsDbContext = classroomsDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            classrooms = new ClassroomsViewModel
            {
                Classrooms = classroomsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                NumberOfClassroom = classroom.NumberOfClassroom
            };
            return View(classrooms);
        }
        [ResponseCache(CacheProfileName = "ModelCache")]
        //[SetToSession("Less")]
        public IActionResult ClassroomByLessonTime(SortState sortOrder, int page = 1)
        {
            ClassroomsViewModel classrooms;
            var classroomByLessonTime = HttpContext.Session.Get<ClassroomsViewModel>("ClassroomByLessonTime");
            if (classroomByLessonTime == null)
            {
                classroomByLessonTime = new ClassroomsViewModel();

            }
            IQueryable<Classroom> classroomsDbContext = _context.Classrooms;
            classroomsDbContext = Sort_SearchByTime(classroomsDbContext, sortOrder, classroomByLessonTime.ClassroomType, classroomByLessonTime.LessonTime, classroomByLessonTime.LessonDate);
            // Разбиение на страницы
            var count = classroomsDbContext.Count();
            classroomsDbContext = classroomsDbContext.Skip((page - 1) * pageSize).Take(pageSize);

            classrooms = new ClassroomsViewModel
            {
                Classrooms = classroomsDbContext,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                LessonTime = classroomByLessonTime.LessonTime,
                ClassroomType = classroomByLessonTime.ClassroomType,
                LessonDate = classroomByLessonTime.LessonDate
            };
            return View(classrooms);
        }
        [HttpPost]
        public IActionResult Index(ClassroomsViewModel classroom)
        {

            HttpContext.Session.Set("Classroom", classroom);

            return RedirectToAction("Index");
        }
            
        [HttpPost]
        public IActionResult ClassroomByLessonTime(ClassroomsViewModel classroom)
        {

            HttpContext.Session.Set("ClassroomByLessonTime", classroom);

            return RedirectToAction("ClassroomByLessonTime");
        }
        // GET: Classrooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classrooms == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(m => m.ClassroomId == id);
            if (classroom == null)
            {
                return NotFound();
            }

            return View(classroom);
        }

        // GET: Classrooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassroomId,NumberOfClassroom,Places,Wing,ClassroomType")] Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classroom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classroom);
        }

        // GET: Classrooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classrooms == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }
            return View(classroom);
        }

        // POST: Classrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassroomId,NumberOfClassroom,Places,Wing,ClassroomType")] Classroom classroom)
        {
            if (id != classroom.ClassroomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classroom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassroomExists(classroom.ClassroomId))
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
            return View(classroom);
        }

        // GET: Classrooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classrooms == null)
            {
                return NotFound();
            }

            var classroom = await _context.Classrooms
                .FirstOrDefaultAsync(m => m.ClassroomId == id);
            if (classroom == null)
            {
                return NotFound();
            }

            return View(classroom);
        }

        // POST: Classrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classrooms == null)
            {
                return Problem("Entity set 'LessonsDbContext.Classrooms'  is null.");
            }
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom != null)
            {
                _context.Classrooms.Remove(classroom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomExists(int id)
        {
          return (_context.Classrooms?.Any(e => e.ClassroomId == id)).GetValueOrDefault();
        }
        private IQueryable<Classroom> Sort_Search(IQueryable<Classroom> classrooms, SortState sortOrder, int searchClassroom)
        {
            switch (sortOrder)
            {
                case SortState.ClassroomAsc:
                    classrooms = classrooms.OrderBy(s => s.NumberOfClassroom);
                    break;
                case SortState.ClassroomDesc:
                    classrooms = classrooms.OrderByDescending(s => s.NumberOfClassroom);
                    break;
            }
            classrooms = classrooms.Where(o => o.NumberOfClassroom == searchClassroom || searchClassroom == 0);

            return classrooms;
        }
        private IQueryable<Classroom> Sort_SearchByTime(IQueryable<Classroom> classrooms, SortState sortOrder, string searchClassroomType, TimeSpan searchLessonTime,DateTime searchDate)
        {
            switch (sortOrder)
            {
                case SortState.ClassroomAsc:
                    classrooms = classrooms.OrderBy(s => s.NumberOfClassroom);
                    break;
                case SortState.ClassroomDesc:
                    classrooms = classrooms.OrderByDescending(s => s.NumberOfClassroom);
                    break;
            }
            var unavailableClassrooms =classrooms
            .Where(c => !c.Lessons.Any(l =>
                l.LessonTime.LessonTime == searchLessonTime  
                && l.LessonDate == searchDate
                && l.Classroom.ClassroomType.Contains(searchClassroomType )));

            return unavailableClassrooms;
        }
    }
}

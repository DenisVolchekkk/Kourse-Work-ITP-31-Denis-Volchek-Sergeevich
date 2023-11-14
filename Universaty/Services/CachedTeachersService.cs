
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laba3.Models;
namespace Services
{
    public class CachedLessonsService : ICachedLessonsService
    {
        private readonly LessonsDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private int time = 2 * 4 * 240;
        public CachedLessonsService(LessonsDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }
        // получение списка емкостей из базы
        public IEnumerable<Lesson> GetLessons(int rowsNumber = 20)
        {
  
            return _dbContext.Lessons.Include("Discipline").Include("Classroom").Include("DisciplineType").Include("LessonTime").Include("StudentsGroup").Include("Teacher").Take(rowsNumber).ToList();
        }
        public IEnumerable<Lesson> GetLessonsByTeacherName(string teacherName, int rowsNumber = 20)
        {
            return _dbContext.Lessons
                .Include("Discipline")
                .Include("Classroom")
                .Include("DisciplineType")
                .Include("LessonTime")
                .Include("StudentsGroup")
                .Include("Teacher").Take(rowsNumber)
                .Where(lesson => lesson.Teacher.TeacherName.Contains(teacherName))
                .ToList();
        }
        // добавление списка учителей в кэш
        public void AddLessons(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Lesson> Lessons = _dbContext.Lessons.Include("Discipline").Include("Classroom").Include("DisciplineType").Include("LessonTime").Include("StudentsGroup").Include("Teacher").Take(rowsNumber).ToList();
            if (Lessons != null)
            {
                _memoryCache.Set(cacheKey, Lessons, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(time)
                });
                Console.WriteLine("Список учителей добавлен в кэш");
            }

        }

        // получение списка учителей из кэша или из базы, если нет в кэше
        public IEnumerable<Lesson> GetLessons(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Lesson> Lessons;
            if (!_memoryCache.TryGetValue(cacheKey, out Lessons))
            {
                Lessons = _dbContext.Lessons.Include("Discipline").Include("Classroom").Include("DisciplineType").Include("LessonTime").Include("StudentsGroup").Include("Teacher").Take(rowsNumber).ToList();
                if (Lessons != null)
                {
                    _memoryCache.Set(cacheKey, Lessons,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(time)));
                }
            }
            Console.WriteLine("Список учителей получен из кэша");
            return Lessons;
        }

    }
}
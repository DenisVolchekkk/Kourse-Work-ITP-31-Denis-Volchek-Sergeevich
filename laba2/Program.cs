using Universaty.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using laba2.Models;
using Lesson = Universaty.Domain.Lesson;
using Teacher = Universaty.Domain.Teacher;
using System.Diagnostics.Metrics;

namespace laba2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new SchoolContext();
            int key = 0;
            do
            {
                Console.WriteLine("1. Выборка дисплин");
                Console.WriteLine("2. Выборка классов с кол-вом мест > 80");
                Console.WriteLine("3. Среднее количество стундтов в группе на факультетах");
                Console.WriteLine("4. Выборка данных из двух полей двух таблиц, связанных между собой отношением «один-ко-многим» ");
                Console.WriteLine("5. Выборка данных из двух таблиц, связанных между собой отношением «один-ко-многим» и отфильтрованным по некоторому условию, налагающему ограничения на значения одного или нескольких полей ");
                Console.WriteLine("6. Вставка данных в таблицу Teachers");
                Console.WriteLine("7. Удаление данных из таблицы Teachers");
                Console.WriteLine("8. Вставка данных в таблицу Lesson");
                Console.WriteLine("9. Удаление данных из таблицы Lesson ");
                Console.WriteLine("10. Обновление удовлетворяющих определенному условию записей в таблице Discipline ");
                Console.WriteLine("11. Выход");
                key = int.Parse(Console.ReadLine());
                switch (key)
                {
                    case 1:
                        SelectDisciplines(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 2:
                        WherePlaces(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 3:
                        GroupByStudentsGroups(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 4:
                        JoinTeacher(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 5:
                        SelectLessonAndStudentQuantity(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 6:
                        AddTeacher(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 7:
                        DeleteTeacher(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 8:
                        AddLesson(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 9:
                        DeleteLesson(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 10:
                        UpdateDiscipline(dbContext);
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 11:
                        Environment.Exit(0) ;
                        break;
                    default:
                        break;
                }
            } while (key != 11);


        }
        //Выборка всех данных из таблицы, стоящей в схеме базы данных нас стороне отношения «один» – 1 шт.
        static void SelectDisciplines(SchoolContext dbContext)
        {
            var disciplines = dbContext.Disciplines.ToList();

            // Выводим результаты выборки
            foreach (var discipline in disciplines)
            {
                Console.WriteLine($"|DisciplineID: {discipline.DisciplineID}\t| DisciplineName: {discipline.DisciplineName}\t");
            }
        }
        //Выборка данных из таблицы, стоящей в схеме базы данных нас стороне отношения «один»,
        static void WherePlaces(SchoolContext dbContext)
        {

            int targetPlaces = 80;

            var disciplines = dbContext.Classrooms
                .Where(d => d.Places > 80)
                .ToList();

            foreach (var classroom in disciplines)
            {
                Console.WriteLine($"|ClassroomId: {classroom.ClassroomID}\t| ClassroomPlaces: {classroom.Places}\t|");
            }
        }
        static void GroupByStudentsGroups(SchoolContext dbContext)
        {
            var groupStatistics = dbContext.StudentsGroups
            .GroupBy(g => g.FacilityId)
            .Select(g => new
            {
                FacilityId = g.Key,
                AverageQuantityOfStudents = g.Average(g => g.QuantityOfStudents)
            })
            .ToList();

            foreach (var statistics in groupStatistics)
            {
                Console.WriteLine($"|FacilityId: {statistics.FacilityId}\t| AverageQuantityOfStudents: {(int)statistics.AverageQuantityOfStudents}\t|");
            }
        }
        static void JoinTeacher(SchoolContext dbContext)
        {
            var query = from lesson in dbContext.Lessons
                        join teacher in dbContext.Teachers on lesson.TeacherID equals teacher.TeacherID
                        select new
                        {
                            LessonID = lesson.LessonID,
                            LessonDate = lesson.LessonDate,
                            TeacherName = teacher.TeacherName
                        };

            var result = query.ToList();

            foreach (var item in result)
            {
                Console.WriteLine($"|LessonID: {item.LessonID}\t|LessonDate: {item.LessonDate}\t| TeacherName: {item.TeacherName}\t|");
            }
        }
        static void SelectLessonAndStudentQuantity(SchoolContext dbContext)
        {
            string keyword = "БЖЧ"; // Ключевое слово для фильтрации дисциплин
            int minStudents = 15; // Минимальное количество студентов в группе

            var query = from lesson in dbContext.Lessons.Include(l => l.Discipline).Include(l => l.StudentsGroup)
                        where lesson.Discipline.DisciplineName.Contains(keyword)
                              && lesson.StudentsGroup.QuantityOfStudents > minStudents
                        select lesson;


            var result = query.ToList();

            foreach (var lesson in result)
            {
                Console.WriteLine($"LessonID: {lesson.LessonID}\t|Discipline: {lesson.Discipline.DisciplineName}| Students: {lesson.StudentsGroup.QuantityOfStudents}|");
            }
        }
        static void AddTeacher(SchoolContext dbContext)
        {
            Console.WriteLine("Введите имя учителя");
            string name = Console.ReadLine();
            // Создание нового объекта Teacher
            var teacher = new Teacher
            {
                TeacherName = name // Пример значения для TeacherName
            };

            // Добавление объекта Teacher в контекст данных
            dbContext.Teachers.Add(teacher);

            // Сохранение изменений
            dbContext.SaveChanges();

            Console.WriteLine("Учитель добавлен");
        }
        static void DeleteTeacher(SchoolContext dbContext)
        {
            Console.WriteLine("Введите имя учителя");
            string name = Console.ReadLine();
            // Найти учителя, которого нужно удалить
            var teacher = dbContext.Teachers.FirstOrDefault(t => t.TeacherName == name);

            if (teacher != null)
            {
                // Удаление объекта Teacher из контекста данных
                dbContext.Teachers.Remove(teacher);

                // Сохранение изменений
                dbContext.SaveChanges();

                Console.WriteLine("Учитель удален");
            }
            
        }

        static void AddLesson(SchoolContext dbContext)
        {
            var lesson = new Lesson
            {
                // Заполнение остальных свойств объекта Lesson
                DisciplineID = 1,
                ClassroomID = 1,
                DisciplineTypeID = 1,
                TeacherID = 1,
                StudentsGroupID = 1,
                Semestr = 1,
                LessonDate = DateTime.Now,
                LessonTimeID = 1,
                Year = 2023,
                DayOfWeek = 1
            };
            dbContext.Lessons.Add(lesson);

            // Сохранение изменений
            dbContext.SaveChanges();
            Console.WriteLine("Урок добавлен");
        }
        
        static void DeleteLesson(SchoolContext dbContext)
        {
            Console.WriteLine("Введите id");
            int id = int.Parse(Console.ReadLine());
            var lesson = dbContext.Lessons.Find(id);

            if (lesson != null)
            {
                // Remove the lesson from the context
                dbContext.Lessons.Remove(lesson);

                // Save the changes
                dbContext.SaveChanges();
                Console.WriteLine("Урок удален");
            }
        }
        static void UpdateDiscipline(SchoolContext dbContext)
        {
            Console.WriteLine("Введите название дисциплины, которую хотите изменить");
            string name = Console.ReadLine();
            Console.WriteLine("Введите новое название дисциплины");
            string newName = Console.ReadLine();
            var disciplinesToUpdate = from d in dbContext.Disciplines
                                      where d.DisciplineName == name
                                      select d;

            foreach (var discipline in disciplinesToUpdate)
            {
                discipline.DisciplineName = newName;
                // Обновить другие свойства, если необходимо
            }

            dbContext.SaveChanges();
            Console.WriteLine("Обновление успешно!");
            }
        }

    }

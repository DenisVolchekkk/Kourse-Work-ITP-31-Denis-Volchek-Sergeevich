
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Tasks;
using Univercity.Domain;

namespace Univercity.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(LessonsDbContext db)
        {
            db.Database.EnsureCreated();

 

            int teacher_number = 100;
            int studentGroup_number = 100;
            int classroom_number = 100;
            int lessons_number = 1000;
            TimeSpan lessonTime;
            string[] disciplineNames_voc = { ("ООП"), ("Математический анализ"), ("Английский язык"), ("БЖЧ"), ("Теория вероятности"), ("Дискретная математика"), ("Кластерный анализ"), ("Базы данных"), ("История"), ("Философия") };//словарь названий емкостей
            string[] lessonsTimes_voc = { ("8:20:00"), ("10:00:00"), ("11:35:00"), ("13:30:00"), ("15:05:00"), ("16:45:00"), ("18:20:00"), ("20:50:00") };//словарь названий емкостей
            string[] disciplineTypes_voc = { ("Лекция"), ("Практика"), ("Лабораторная работа") };
            string[] facilities_voc = { ("ФАИС"), ("ГЭФ"), ("Машфак"), ("Энергофак"), ("МТФ") };

            if (!db.Disciplines.Any() && !db.LessonsTimes.Any() && !db.DisciplineTypes.Any()
                && !db.Classrooms.Any() && !db.Teachers.Any() && !db.Facilities.Any()
                && !db.Lessons.Any() && !db.StudentsGroups.Any())
            {
                Random randObj = new(1);
                Random random = new Random();
                if (!db.LessonsTimes.Any())
                {

                    for (int lessonTimeID = 1; lessonTimeID <= lessonsTimes_voc.Length; lessonTimeID++)
                    {
                        lessonTime = TimeSpan.ParseExact(lessonsTimes_voc[lessonTimeID - 1], "h\\:mm\\:ss", CultureInfo.InvariantCulture);
                        db.LessonsTimes.Add(new LessonsTime { LessonTime = lessonTime });
                    }
                    //сохранение изменений в базу данных, связанную с объектом контекста
                    db.SaveChanges();
                }
                if (!db.Teachers.Any())
                {
                    string[] teacherFirstName_voc = { ("Денис"), ("Игорь"), ("Михаил"), ("Андрей"), ("Владимир"), ("Владислав"), ("Алексей"), ("Кирилл"), ("Григорий"), ("Иван") };
                    string[] teacherSecondName_voc = { ("Волчек"), ("Суворов"), ("Сидоров"), ("Петров"), ("Моиссенко"), ("Маслак"), ("Бондарев"), ("Данилков"), ("Иванов"), ("Суковатый") };
                    string[] teacherMiddleName_voc = { ("Денисович"), ("Игороревич"), ("Михаилович"), ("Андревич"), ("Владимирович"), ("Владиславович"), ("Алексеевич"), ("Кириллович"), ("Григорьевич"), ("Иванович") };
                    int count_teacherFirstName_voc = teacherFirstName_voc.GetLength(0);
                    int count_teacherSecondName_voc = teacherSecondName_voc.GetLength(0);
                    int count_teacherMiddleName_voc = teacherMiddleName_voc.GetLength(0);
                    string teacherName;
                    for (int teacherID = 1; teacherID <= teacher_number; teacherID++)
                    {
                        teacherName = teacherSecondName_voc[randObj.Next(count_teacherSecondName_voc)] + " " + teacherFirstName_voc[randObj.Next(count_teacherFirstName_voc)] + " " +
                            teacherMiddleName_voc[randObj.Next(count_teacherMiddleName_voc)];
                        db.Teachers.Add(new Teacher { TeacherName = teacherName });
                    }
                    db.SaveChanges();
                }
                if (!db.Facilities.Any())
                {
                    string facilityName;
                    for (int facilityID = 1; facilityID <= facilities_voc.Length; facilityID++)
                    {
                        facilityName = facilities_voc[facilityID - 1];
                        db.Facilities.Add(new Facility { FacilityName = facilityName });
                    }
                    //сохранение изменений в базу данных, связанную с объектом контекста
                    db.SaveChanges();
                }
                if (!db.StudentsGroups.Any())
                {
                    string[] studentGroups_voc = { "ИТП", "ИТИ", "МЭф", "ШЭФ", "МАТ", "ИТ", "СХ", "МАЗ", "ТПЗ", "ФЛК", "МАТ" };
                    int count_studentGroups_voc = studentGroups_voc.GetLength(0);
                    for (int studentGroupID = 1; studentGroupID <= studentGroup_number; studentGroupID++)
                    {
                        db.StudentsGroups.Add(new StudentsGroup { NumberOfGroup = studentGroups_voc[randObj.Next(count_studentGroups_voc)] + "-" + studentGroupID.ToString(), QuantityOfStudents = random.Next(15, 31), FacilityId = random.Next(1, (facilities_voc.Length + 1)) });
                    }
                    db.SaveChanges();
                }
                if (!db.Classrooms.Any())
                {
                    string[] classroomTypes_voc = { ("Аудитория"), ("Кабинет") };
                    for (int classroomID = 1; classroomID <= classroom_number; classroomID++)
                    {
                        db.Classrooms.Add(new Classroom { NumberOfClassroom = classroomID, Places = random.Next(40, 121), Wing = random.Next(1, 4), ClassroomType = classroomTypes_voc[random.Next(0, 2)] });
                    }
                    db.SaveChanges();
                }
                if (!db.DisciplineTypes.Any())
                {
                    string disciplineType;
                    for (int disciplineTypeID = 1; disciplineTypeID <= disciplineTypes_voc.Length; disciplineTypeID++)
                    {
                        disciplineType = disciplineTypes_voc[disciplineTypeID - 1];
                        db.DisciplineTypes.Add(new DisciplineType { TypeOfDiscipline = disciplineType });
                    }
                    db.SaveChanges();
                }
                if (!db.Disciplines.Any())
                {
                    string disciplineName;
                    for (int disciplineNameID = 1; disciplineNameID <= disciplineNames_voc.Length; disciplineNameID++)
                    {
                        disciplineName = disciplineNames_voc[disciplineNameID - 1];
                        db.Disciplines.Add(new Discipline { DisciplineName = disciplineName });
                    }
                    db.SaveChanges();
                }
                if (!db.Lessons.Any())
                {
                    for (int lessonID = 1; lessonID <= lessons_number; lessonID++)
                    {
                        DateTime start = new DateTime(2023, 1, 1);
                        int range = (DateTime.Today - start).Days;
                        DateTime randomDate = start.AddDays(random.Next(range)).AddYears(random.Next(-2,2));
                        db.Lessons.Add(new Lesson
                        {
                            DisciplineId = random.Next(1, (disciplineNames_voc.Length + 1)),
                            DisciplineTypeId = random.Next(1, (disciplineTypes_voc.Length + 1)),
                            ClassroomId = random.Next(1, (classroom_number + 1)),
                            TeacherId = random.Next(1, (teacher_number + 1)),
                            StudentsGroupId = random.Next(1, (studentGroup_number + 1)),
                            LessonTimeId = random.Next(1, (lessonsTimes_voc.Length + 1)),
                            Semestr = random.Next(1, 3),
                            Year = randomDate.Year,
                            LessonDate = randomDate,
                            DayOfweek = ((int)randomDate.DayOfWeek + 6) % 7 + 1
                        });
                    }
                    db.SaveChanges();
                }
            }
        }
    }
}

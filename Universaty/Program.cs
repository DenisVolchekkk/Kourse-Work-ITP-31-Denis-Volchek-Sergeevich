using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Laba3.Models;
using Services;
using System.Text.Json;
using Microsoft.AspNetCore.Html;

namespace Universaty
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            // внедрение зависимости для доступа к БД с использованием EF
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<LessonsDbContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();
            // добавление кэширования
            services.AddMemoryCache();

            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            // внедрение зависимости CachedTanksService
            services.AddScoped<ICachedLessonsService, CachedLessonsService>();

            //Использование MVC - отключено
            //services.AddControllersWithViews();
            var app = builder.Build();
            // добавляем поддержку статических файлов
            app.UseStaticFiles();

            // добавляем поддержку сессий
            app.UseSession();

            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Формирование строки для вывода 
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BR>";
                    strResponse += "<BR><A href='/Lessons'>Уроки</A></BR>";
                    strResponse += "<BR><A href='/info'>Данные пользователя</A></BR>";
                    strResponse += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                    strResponse += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                    strResponse += "</BODY></HTML>";
                    // Вывод данных
                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.Map("/Lessons", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> Lessons = cachedLessonsService.GetLessons("Lessons20");
                    string HtmlString = "<HTML><HEAD><TITLE>Уроки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список уроков</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Дисциплина</TH>";
                    HtmlString += "<TH>Номер аудитории</TH>";
                    HtmlString += "<TH>Тип дисциплины</TH>";
                    HtmlString += "<TH>Учитель</TH>";
                    HtmlString += "<TH>Группа студентов</TH>";
                    HtmlString += "<TH>Семестр</TH>";
                    HtmlString += "<TH>Дата урока</TH>";
                    HtmlString += "<TH>Время урока</TH>";
                    HtmlString += "<TH>Год</TH>";
                    HtmlString += "<TH>День недели</TH>";
                    HtmlString += "</TR>";
                    foreach (var Lesson in Lessons) { 
                            HtmlString += "<TR>";
                        HtmlString += "<TD>" + Lesson.LessonId + "</TD>";
                        HtmlString += "<TD>" + Lesson.Discipline.DisciplineName + "</TD>";
                        HtmlString += "<TD>" + Lesson.Classroom.NumberOfClassroom + "</TD>";
                        HtmlString += "<TD>" + Lesson.DisciplineType.TypeOfDiscipline + "</TD>";
                        HtmlString += "<TD>" + Lesson.Teacher.TeacherName + "</TD>";
                        HtmlString += "<TD>" + Lesson.StudentsGroup.NumberOfGroup + "</TD>";
                        HtmlString += "<TD>" + Lesson.Semestr + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonDate + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonTime.LessonTime + "</TD>";
                        HtmlString += "<TD>" + Lesson.Year + "</TD>";
                        HtmlString += "<TD>" + Lesson.DayOfweek + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>Уроки</A></BR>";
                    HtmlString += "<BR><A href='/info'>Данные пользователя</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/SeacrhForm1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> Lessons = cachedLessonsService.GetLessons("Lessons20");
                    string HtmlString = "<HTML><HEAD><TITLE>Уроки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<form method='get' action='/SearchTeacher1'>";
                    if (context.Request.Cookies.TryGetValue("teacherName", out string savedTeacherName))
                    {
                        // Установка сохраненного значения в поле ввода формы
                        HtmlString += "<input type='text' name='teacherName' placeholder='Введите имя учителя' value='" + savedTeacherName + "' />";
                    }
                    else
                    {
                        // Если сохраненного значения нет, установите пустое значение в поле ввода формы
                        HtmlString += "<input type='text' name='teacherName' placeholder='Введите имя учителя' />";
                    }
                    HtmlString += "<button type='submit'>Поиск</button>" +
                    "</form>" +
                    "<BODY><H1>Список уроков</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Дисциплина</TH>";
                    HtmlString += "<TH>Номер аудитории</TH>";
                    HtmlString += "<TH>Тип дисциплины</TH>";
                    HtmlString += "<TH>Учитель</TH>";
                    HtmlString += "<TH>Группа студентов</TH>";
                    HtmlString += "<TH>Семестр</TH>";
                    HtmlString += "<TH>Дата урока</TH>";
                    HtmlString += "<TH>Время урока</TH>";
                    HtmlString += "<TH>Год</TH>";
                    HtmlString += "<TH>День недели</TH>";
                    HtmlString += "</TR>";
                    foreach (var Lesson in Lessons)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + Lesson.LessonId + "</TD>";
                        HtmlString += "<TD>" + Lesson.Discipline.DisciplineName + "</TD>";
                        HtmlString += "<TD>" + Lesson.Classroom.NumberOfClassroom + "</TD>";
                        HtmlString += "<TD>" + Lesson.DisciplineType.TypeOfDiscipline + "</TD>";
                        HtmlString += "<TD>" + Lesson.Teacher.TeacherName + "</TD>";
                        HtmlString += "<TD>" + Lesson.StudentsGroup.NumberOfGroup + "</TD>";
                        HtmlString += "<TD>" + Lesson.Semestr + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonDate + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonTime.LessonTime + "</TD>";
                        HtmlString += "<TD>" + Lesson.Year + "</TD>";
                        HtmlString += "<TD>" + Lesson.DayOfweek + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>Уроки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                    HtmlString += "<BR><A href='/info'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/SeacrhForm2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //обращение к сервису
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> Lessons = cachedLessonsService.GetLessons("Lessons20");
                    string json = File.ReadAllText("session.json");

                    string HtmlString = "<HTML><HEAD><TITLE>Уроки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<form method='get' action='/SearchTeacher2'>";
                    if (context.Session.TryGetValue("SearchFormState", out byte[] searchFormStateBytes))
                    {
                        // Десериализация массива байтов в объект
                        var searchFormState = JsonSerializer.Deserialize<SearchFormState>(json);
                        Console.WriteLine(searchFormState.TeacherName);
                        // Установка значения из объекта в поле ввода формы
                        HtmlString += "<input type='text' name='teacherName' placeholder='Введите имя учителя' value='" + searchFormState.TeacherName + "' />";
                    }
                    else
                    {
                        // Если сохраненного объекта нет, установите пустое значение в поле ввода формы
                        HtmlString += "<input type='text' name='teacherName' placeholder='Введите имя учителя' />";
                    }
                    HtmlString += "<button type='submit'>Поиск</button>" +
                    "</form>" +
                    "<BODY><H1>Список уроков</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Дисциплина</TH>";
                    HtmlString += "<TH>Номер аудитории</TH>";
                    HtmlString += "<TH>Тип дисциплины</TH>";
                    HtmlString += "<TH>Учитель</TH>";
                    HtmlString += "<TH>Группа студентов</TH>";
                    HtmlString += "<TH>Семестр</TH>";
                    HtmlString += "<TH>Дата урока</TH>";
                    HtmlString += "<TH>Время урока</TH>";
                    HtmlString += "<TH>Год</TH>";
                    HtmlString += "<TH>День недели</TH>";
                    HtmlString += "</TR>";
                    foreach (var Lesson in Lessons)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + Lesson.LessonId + "</TD>";
                        HtmlString += "<TD>" + Lesson.Discipline.DisciplineName + "</TD>";
                        HtmlString += "<TD>" + Lesson.Classroom.NumberOfClassroom + "</TD>";
                        HtmlString += "<TD>" + Lesson.DisciplineType.TypeOfDiscipline + "</TD>";
                        HtmlString += "<TD>" + Lesson.Teacher.TeacherName + "</TD>";
                        HtmlString += "<TD>" + Lesson.StudentsGroup.NumberOfGroup + "</TD>";
                        HtmlString += "<TD>" + Lesson.Semestr + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonDate + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonTime.LessonTime + "</TD>";
                        HtmlString += "<TD>" + Lesson.Year + "</TD>";
                        HtmlString += "<TD>" + Lesson.DayOfweek + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>Уроки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                    HtmlString += "<BR><A href='/info'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/SearchTeacher1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение значения параметра "teacherName" из запроса
                    string teacherName = context.Request.Query["teacherName"];
                    Console.WriteLine(teacherName);
                    // Создание объекта куки
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1), // Установите длительное время жизни куки (1 год в данном примере)
                        IsEssential = true // Установите IsEssential в true, чтобы куки сохранялись даже при настройке GDPR
                    };

                    // Сохранение значения параметра в куки
                    context.Response.Cookies.Append("teacherName", teacherName, cookieOptions);
                    // Выполнение поиска учителя по имени
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> lessons = cachedLessonsService.GetLessonsByTeacherName(teacherName);
                    string HtmlString = "<HTML><HEAD><TITLE>Уроки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список уроков</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Дисциплина</TH>";
                    HtmlString += "<TH>Номер аудитории</TH>";
                    HtmlString += "<TH>Тип дисциплины</TH>";
                    HtmlString += "<TH>Учитель</TH>";
                    HtmlString += "<TH>Группа студентов</TH>";
                    HtmlString += "<TH>Семестр</TH>";
                    HtmlString += "<TH>Дата урока</TH>";
                    HtmlString += "<TH>Время урока</TH>";
                    HtmlString += "<TH>Год</TH>";
                    HtmlString += "<TH>День недели</TH>";
                    HtmlString += "</TR>";
                    foreach (var Lesson in lessons)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + Lesson.LessonId + "</TD>";
                        HtmlString += "<TD>" + Lesson.Discipline.DisciplineName + "</TD>";
                        HtmlString += "<TD>" + Lesson.Classroom.NumberOfClassroom + "</TD>";
                        HtmlString += "<TD>" + Lesson.DisciplineType.TypeOfDiscipline + "</TD>";
                        HtmlString += "<TD>" + Lesson.Teacher.TeacherName + "</TD>";
                        HtmlString += "<TD>" + Lesson.StudentsGroup.NumberOfGroup + "</TD>";
                        HtmlString += "<TD>" + Lesson.Semestr + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonDate + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonTime.LessonTime + "</TD>";
                        HtmlString += "<TD>" + Lesson.Year + "</TD>";
                        HtmlString += "<TD>" + Lesson.DayOfweek + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>Уроки</A></BR>";
                    HtmlString += "<BR><A href='/info'>Данные пользователя</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/SearchTeacher2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение значения параметра "teacherName" из запроса
                    string teacherName = context.Request.Query["teacherName"];

                    // Создание объекта специальной структуры
                    var searchFormState = new SearchFormState
                    {
                        TeacherName = teacherName
                    };
                    string json = JsonSerializer.Serialize(searchFormState);
                    // Запись JSON в файл
                    File.WriteAllText("session.json", json);
                    var searchFormStateBytes = JsonSerializer.SerializeToUtf8Bytes(searchFormState);
                    // Сохранение объекта в объекте Session
                    context.Session.Set("SearchFormState", searchFormStateBytes);
                    // Сохранение значения параметра в куки
                    // Выполнение поиска учителя по имени
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> lessons = cachedLessonsService.GetLessonsByTeacherName(teacherName);
                    string HtmlString = "<HTML><HEAD><TITLE>Уроки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список уроков</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Код</TH>";
                    HtmlString += "<TH>Дисциплина</TH>";
                    HtmlString += "<TH>Номер аудитории</TH>";
                    HtmlString += "<TH>Тип дисциплины</TH>";
                    HtmlString += "<TH>Учитель</TH>";
                    HtmlString += "<TH>Группа студентов</TH>";
                    HtmlString += "<TH>Семестр</TH>";
                    HtmlString += "<TH>Дата урока</TH>";
                    HtmlString += "<TH>Время урока</TH>";
                    HtmlString += "<TH>Год</TH>";
                    HtmlString += "<TH>День недели</TH>";
                    HtmlString += "</TR>";
                    foreach (var Lesson in lessons)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + Lesson.LessonId + "</TD>";
                        HtmlString += "<TD>" + Lesson.Discipline.DisciplineName + "</TD>";
                        HtmlString += "<TD>" + Lesson.Classroom.NumberOfClassroom + "</TD>";
                        HtmlString += "<TD>" + Lesson.DisciplineType.TypeOfDiscipline + "</TD>";
                        HtmlString += "<TD>" + Lesson.Teacher.TeacherName + "</TD>";
                        HtmlString += "<TD>" + Lesson.StudentsGroup.NumberOfGroup + "</TD>";
                        HtmlString += "<TD>" + Lesson.Semestr + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonDate + "</TD>";
                        HtmlString += "<TD>" + Lesson.LessonTime.LessonTime + "</TD>";
                        HtmlString += "<TD>" + Lesson.Year + "</TD>";
                        HtmlString += "<TD>" + Lesson.DayOfweek + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>Уроки</A></BR>";
                    HtmlString += "<BR><A href='/info'>Данные пользователя</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // Вывод данных
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            // Стартовая страница и кэширование данных таблицы на web-сервере
            app.Run((context) =>
            {
                //обращение к сервису
                ICachedLessonsService cachedTanksService = context.RequestServices.GetService<ICachedLessonsService>();
                cachedTanksService.AddLessons("Lessons20");
                string HtmlString = "<HTML><HEAD><TITLE></TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>Главная</H1>";
                HtmlString += "<H2>Данные записаны в кэш сервера</H2>";
                HtmlString += "<BR><A href='/'>Главная</A></BR>";
                HtmlString += "<BR><A href='/Lessons'>Уроки</A></BR>";
                HtmlString += "<BR><A href='/info'>Данные пользователя</A></BR>";
                HtmlString += "<BR><A href='/SeacrhForm1'>Поиск по имени учителя через куки</A></BR>";
                HtmlString += "<BR><A href='/SeacrhForm2'>Поиск по имени учителя через сессию</A></BR>";
                HtmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(HtmlString);

            });

            //Использование MVC - отключено
            //app.UseRouting();
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

            app.Run();
        }
    }
    public class SearchFormState
    {
        public string TeacherName { get; set; }
    }
}
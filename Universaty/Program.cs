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
            // ��������� ����������� ��� ������� � �� � �������������� EF
            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<LessonsDbContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();
            // ���������� �����������
            services.AddMemoryCache();

            // ���������� ��������� ������
            services.AddDistributedMemoryCache();
            services.AddSession();

            // ��������� ����������� CachedTanksService
            services.AddScoped<ICachedLessonsService, CachedLessonsService>();

            //������������� MVC - ���������
            //services.AddControllersWithViews();
            var app = builder.Build();
            // ��������� ��������� ����������� ������
            app.UseStaticFiles();

            // ��������� ��������� ������
            app.UseSession();

            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ������������ ������ ��� ������ 
                    string strResponse = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>����������:</H1>";
                    strResponse += "<BR> ������: " + context.Request.Host;
                    strResponse += "<BR> ����: " + context.Request.PathBase;
                    strResponse += "<BR> ��������: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>�������</A></BR>";
                    strResponse += "<BR><A href='/Lessons'>�����</A></BR>";
                    strResponse += "<BR><A href='/info'>������ ������������</A></BR>";
                    strResponse += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                    strResponse += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                    strResponse += "</BODY></HTML>";
                    // ����� ������
                    await context.Response.WriteAsync(strResponse);
                });
            });
            app.Map("/Lessons", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //��������� � �������
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> Lessons = cachedLessonsService.GetLessons("Lessons20");
                    string HtmlString = "<HTML><HEAD><TITLE>�����</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ������</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>����������</TH>";
                    HtmlString += "<TH>����� ���������</TH>";
                    HtmlString += "<TH>��� ����������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>������ ���������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>���� �����</TH>";
                    HtmlString += "<TH>����� �����</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>�����</A></BR>";
                    HtmlString += "<BR><A href='/info'>������ ������������</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // ����� ������
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/SeacrhForm1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //��������� � �������
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> Lessons = cachedLessonsService.GetLessons("Lessons20");
                    string HtmlString = "<HTML><HEAD><TITLE>�����</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<form method='get' action='/SearchTeacher1'>";
                    if (context.Request.Cookies.TryGetValue("teacherName", out string savedTeacherName))
                    {
                        // ��������� ������������ �������� � ���� ����� �����
                        HtmlString += "<input type='text' name='teacherName' placeholder='������� ��� �������' value='" + savedTeacherName + "' />";
                    }
                    else
                    {
                        // ���� ������������ �������� ���, ���������� ������ �������� � ���� ����� �����
                        HtmlString += "<input type='text' name='teacherName' placeholder='������� ��� �������' />";
                    }
                    HtmlString += "<button type='submit'>�����</button>" +
                    "</form>" +
                    "<BODY><H1>������ ������</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>����������</TH>";
                    HtmlString += "<TH>����� ���������</TH>";
                    HtmlString += "<TH>��� ����������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>������ ���������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>���� �����</TH>";
                    HtmlString += "<TH>����� �����</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>�����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                    HtmlString += "<BR><A href='/info'>������ ������������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // ����� ������
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/SeacrhForm2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    //��������� � �������
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> Lessons = cachedLessonsService.GetLessons("Lessons20");
                    string json = File.ReadAllText("session.json");

                    string HtmlString = "<HTML><HEAD><TITLE>�����</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<form method='get' action='/SearchTeacher2'>";
                    if (context.Session.TryGetValue("SearchFormState", out byte[] searchFormStateBytes))
                    {
                        // �������������� ������� ������ � ������
                        var searchFormState = JsonSerializer.Deserialize<SearchFormState>(json);
                        Console.WriteLine(searchFormState.TeacherName);
                        // ��������� �������� �� ������� � ���� ����� �����
                        HtmlString += "<input type='text' name='teacherName' placeholder='������� ��� �������' value='" + searchFormState.TeacherName + "' />";
                    }
                    else
                    {
                        // ���� ������������ ������� ���, ���������� ������ �������� � ���� ����� �����
                        HtmlString += "<input type='text' name='teacherName' placeholder='������� ��� �������' />";
                    }
                    HtmlString += "<button type='submit'>�����</button>" +
                    "</form>" +
                    "<BODY><H1>������ ������</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>����������</TH>";
                    HtmlString += "<TH>����� ���������</TH>";
                    HtmlString += "<TH>��� ����������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>������ ���������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>���� �����</TH>";
                    HtmlString += "<TH>����� �����</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>�����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                    HtmlString += "<BR><A href='/info'>������ ������������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // ����� ������
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            app.Map("/SearchTeacher1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� �������� ��������� "teacherName" �� �������
                    string teacherName = context.Request.Query["teacherName"];
                    Console.WriteLine(teacherName);
                    // �������� ������� ����
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1), // ���������� ���������� ����� ����� ���� (1 ��� � ������ �������)
                        IsEssential = true // ���������� IsEssential � true, ����� ���� ����������� ���� ��� ��������� GDPR
                    };

                    // ���������� �������� ��������� � ����
                    context.Response.Cookies.Append("teacherName", teacherName, cookieOptions);
                    // ���������� ������ ������� �� �����
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> lessons = cachedLessonsService.GetLessonsByTeacherName(teacherName);
                    string HtmlString = "<HTML><HEAD><TITLE>�����</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ������</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>����������</TH>";
                    HtmlString += "<TH>����� ���������</TH>";
                    HtmlString += "<TH>��� ����������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>������ ���������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>���� �����</TH>";
                    HtmlString += "<TH>����� �����</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>�����</A></BR>";
                    HtmlString += "<BR><A href='/info'>������ ������������</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // ����� ������
                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/SearchTeacher2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� �������� ��������� "teacherName" �� �������
                    string teacherName = context.Request.Query["teacherName"];

                    // �������� ������� ����������� ���������
                    var searchFormState = new SearchFormState
                    {
                        TeacherName = teacherName
                    };
                    string json = JsonSerializer.Serialize(searchFormState);
                    // ������ JSON � ����
                    File.WriteAllText("session.json", json);
                    var searchFormStateBytes = JsonSerializer.SerializeToUtf8Bytes(searchFormState);
                    // ���������� ������� � ������� Session
                    context.Session.Set("SearchFormState", searchFormStateBytes);
                    // ���������� �������� ��������� � ����
                    // ���������� ������ ������� �� �����
                    ICachedLessonsService cachedLessonsService = context.RequestServices.GetService<ICachedLessonsService>();
                    IEnumerable<Lesson> lessons = cachedLessonsService.GetLessonsByTeacherName(teacherName);
                    string HtmlString = "<HTML><HEAD><TITLE>�����</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>������ ������</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>����������</TH>";
                    HtmlString += "<TH>����� ���������</TH>";
                    HtmlString += "<TH>��� ����������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>������ ���������</TH>";
                    HtmlString += "<TH>�������</TH>";
                    HtmlString += "<TH>���� �����</TH>";
                    HtmlString += "<TH>����� �����</TH>";
                    HtmlString += "<TH>���</TH>";
                    HtmlString += "<TH>���� ������</TH>";
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
                    HtmlString += "<BR><A href='/'>�������</A></BR>";
                    HtmlString += "<BR><A href='/Lessons'>�����</A></BR>";
                    HtmlString += "<BR><A href='/info'>������ ������������</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                    HtmlString += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    // ����� ������
                    await context.Response.WriteAsync(HtmlString);
                });
            });
            // ��������� �������� � ����������� ������ ������� �� web-�������
            app.Run((context) =>
            {
                //��������� � �������
                ICachedLessonsService cachedTanksService = context.RequestServices.GetService<ICachedLessonsService>();
                cachedTanksService.AddLessons("Lessons20");
                string HtmlString = "<HTML><HEAD><TITLE></TITLE></HEAD>" +
                "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                "<BODY><H1>�������</H1>";
                HtmlString += "<H2>������ �������� � ��� �������</H2>";
                HtmlString += "<BR><A href='/'>�������</A></BR>";
                HtmlString += "<BR><A href='/Lessons'>�����</A></BR>";
                HtmlString += "<BR><A href='/info'>������ ������������</A></BR>";
                HtmlString += "<BR><A href='/SeacrhForm1'>����� �� ����� ������� ����� ����</A></BR>";
                HtmlString += "<BR><A href='/SeacrhForm2'>����� �� ����� ������� ����� ������</A></BR>";
                HtmlString += "</BODY></HTML>";

                return context.Response.WriteAsync(HtmlString);

            });

            //������������� MVC - ���������
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
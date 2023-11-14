using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Univercity.Models;
using Univercity.Middleware;

namespace Univercity
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            string connection = builder.Configuration.GetConnectionString("SqlServerConnection");

            // ��������� ����������� ��� ������� � �� � �������������� EF
            builder.Services.AddDbContext<LessonsDbContext>(options => options.UseSqlServer(connection));
            builder.Services.AddControllersWithViews(options => {
                options.CacheProfiles.Add("ModelCache",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.Any,
                        Duration = 2 * 4 + 240
                    });
            });

            builder.Services.AddSession();
            
            //������������� MVC - ���������
            //services.AddControllersWithViews();
            var app = builder.Build();
            // ��������� ��������� ����������� ������
            app.UseStaticFiles();
            
            // ��������� ��������� ������
            app.UseSession();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseDbInitializer();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            }); ;


            app.Run();
        }
    }
}
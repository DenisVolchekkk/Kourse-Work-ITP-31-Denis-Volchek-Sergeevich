using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Universaty.Domain;
namespace MiddleWareLib
{
    public static class CacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheMiddleware(this IApplicationBuilder app, int cacheDuration)
        {
            return app.UseMiddleware<CacheMiddleware>(cacheDuration);
        }
    }

    public interface IDbService
    {
        Task<string> GetCachedData();
    }

    public class DbService : IDbService
    {
        private readonly SchoolContext _dbContext;
        private readonly IMemoryCache _cache;

        public DbService(SchoolContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<string> GetCachedData()
        {
            var cacheKey = "CachedData";

            if (_cache.TryGetValue(cacheKey, out string cachedData))
            {
                // Если данные присутствуют в кэше, возвращаем их
                return cachedData;
            }
            else
            {
                // Логика получения данных из базы данных
                var data = await _dbContext.Teachers.Take(20).ToListAsync();

                // Преобразование данных в строку или объект, который можно преобразовать в строку
                //var dataAsString = ConvertDataToString(data);

                // Кэширование данных на указанное время
                var cacheExpiration = TimeSpan.FromMinutes(10); // Пример: кэш действителен в течение 10 минут
                _cache.Set(cacheKey, data, cacheExpiration);

                return data;
            }
        }

        //private IEnumerable<Teacher> ConvertDataToTeacher(List<Teacher> data)
        //{
        //    List<Teacher> users = new List<User>();

        //    foreach (Teacher item in data)
        //    {
        //        Teacher user = new Teacher
        //        {
        //            Name = item.Name
        //            // Другие свойства User, которые вы хотите заполнить из YourDataModel
        //        };

        //        users.Add(user);
        //    }

        //    return users;
        //}
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем IMemoryCache в сервисах
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app)
        {
            // Используем кэш-мидлваре для всех запросов
            app.UseCacheMiddleware(2 *4 +240);

            // Добавьте остальные компоненты конвейера и настройки приложения
        }
    }
}

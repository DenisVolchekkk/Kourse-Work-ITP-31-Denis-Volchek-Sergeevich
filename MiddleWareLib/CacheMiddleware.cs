using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MiddleWareLib
{
    public class CacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly int _cacheDuration;
        private readonly IDbService _dbService;

        public CacheMiddleware(RequestDelegate next, IMemoryCache cache, int cacheDuration, IDbService dbService)
        {
            _next = next;
            _cache = cache;
            _cacheDuration = cacheDuration;
            _dbService = dbService;
        }

        public async Task Invoke(HttpContext context)
        {
            var cacheKey = $"Cache_{context.Request.Path}";

            if (_cache.TryGetValue(cacheKey, out var cachedData))
            {
                // Если данные присутствуют в кэше, возвращаем их
                await context.Response.WriteAsync(cachedData.ToString());
            }
            else
            {
                var data = await _dbService.GetCachedData();

                // Кэшируем данные на указанное время
                _cache.Set(cacheKey, data, TimeSpan.FromSeconds(_cacheDuration));

                // Отправляем данные клиенту
                await context.Response.WriteAsync(data.ToString());
            }
        }
    }

}
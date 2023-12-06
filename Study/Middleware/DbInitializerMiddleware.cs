using Microsoft.AspNetCore.Identity;
using Univercity.Persistence;

namespace Study.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next) => _next = next;
        public Task Invoke(HttpContext context, LessonsDbContext dbContext, ApplicationDbContext identityDb, UserManager<IdentityUser> userManager)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                DbInitializer.Initialize(dbContext);
                UserDbInitializer.Initialize(identityDb, userManager);
                context.Session.SetString("starting", "Yes");
            }
            // Call the next delegate/middleware in the pipeline
            return _next.Invoke(context);
        }
    }

}


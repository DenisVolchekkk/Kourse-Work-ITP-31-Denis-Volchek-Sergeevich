
using Microsoft.AspNetCore.Identity;

namespace Study.Data
{
    public class UserDbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Проверяем, существует ли роль "user"
            var userRoleExists = await roleManager.RoleExistsAsync("user");

            if (!userRoleExists)
            {
                // Создаем роль "user", если она не существует
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            // Проверяем, существует ли роль "admin"
            var adminRoleExists = await roleManager.RoleExistsAsync("admin");

            if (!adminRoleExists)
            {
                // Создаем роль "admin", если она не существует
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
        }
    }
}

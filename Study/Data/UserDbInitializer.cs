using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Study.Models;

namespace Study.Data
{

    public static class UserDbInitializer
    {

        public static void Initialize( ApplicationDbContext identityDb, UserManager<IdentityUser> userManager)
        {

            identityDb.Database.EnsureCreated();
            if (!identityDb.Roles.Any())
            {
                InitializeIdentityRoles(identityDb);
            }
            if (!identityDb.Users.Any())
            {
                InitializeIdentityUser(identityDb,userManager);
            }

        }

        private static void InitializeIdentityRoles(ApplicationDbContext identityDb)
        {
            string[] roles = { "user", "admin" };
            foreach (var name in roles)
            {
                identityDb.Add(new IdentityRole() { Name = name, NormalizedName = name.ToUpper() });
            }
            identityDb.SaveChanges();
        }
        private static async void InitializeIdentityUser(ApplicationDbContext identityDb, UserManager<IdentityUser> userManager)
        {
            string email =  "admin@gmail.com" ;
            string password = "_Aa123456";
            IdentityUser admin = new IdentityUser { Email = email, UserName = email, EmailConfirmed = true };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
            identityDb.SaveChanges();
        }

    }
}

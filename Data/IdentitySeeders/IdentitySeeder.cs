using Hoshi.Enums;
using Hoshi.Models.GlobalModels;
using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hoshi.Data.IdentitySeeders
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            string[] roles = { "SuperAdmin", UserType.Admin.ToString(), UserType.Client.ToString(), UserType.Worker.ToString() };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role, NormalizedName = role.ToUpper() });
                }
            }
        }

        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            var dictionaryAdminUsers = new Dictionary<string, List<string>>
            {
                { "adminEmail" ,new List<string>{"novix@novix.com" , "hoshi@hoshi.com" }} ,
                { "adminPassword" , new List<string>{"Novix@12345" , "Hoshi@12345"}}
            };
            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
                await roleManager.CreateAsync(new IdentityRole<int>("SuperAdmin"));

            if (!await roleManager.RoleExistsAsync(UserType.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole<int>(UserType.Admin.ToString()));

            // Check if admin is exist , if not we will go to create it
            await CheckThenAddAdminUser(userManager, dictionaryAdminUsers["adminEmail"][0], dictionaryAdminUsers["adminPassword"][0]);
            await CheckThenAddAdminUser(userManager, dictionaryAdminUsers["adminEmail"][1], dictionaryAdminUsers["adminPassword"][1]);   
        }
        public static async Task CheckThenAddAdminUser(UserManager<User> userManager,
                                                       string adminEmail , string adminPassword)
        {
            // Check if the admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "SuperAdmin");
                    await userManager.AddToRoleAsync(newAdmin, UserType.Admin.ToString());
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }
        }

        public static async Task SeedNotificationTypesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HoshiDbContext>();

            var notificationTypes = new List<NotificationType>
            {
                    // notifications
                    new NotificationType { Title = "اشعار بإنشاء طلب", Type = "For_Worker", ForClient = false },
                    new NotificationType { Title = "اشعار للاختبار", Type = "For_Client", ForClient = true },
   
            };

            foreach (var notif in notificationTypes)
            {
                bool exists = await context.NotificationTypes
                    .AnyAsync(n => n.Type == notif.Type);

                if (!exists)
                {
                    context.NotificationTypes.Add(notif);
                }
            }

            await context.SaveChangesAsync();
        }

    }
}

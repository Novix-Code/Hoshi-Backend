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

            string[] roles = { "client", "admin", "worker" };

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

            string adminEmail = "admin@example.com";
            string adminPassword = "P@ssw0rd";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("admin"));
            }

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
                    await userManager.AddToRoleAsync(newAdmin, "admin");
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
        new NotificationType { Title = "اشعار بإنشاء طلب", Type = "For_Admin", ForClient = false },
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

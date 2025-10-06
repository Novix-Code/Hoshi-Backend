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

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
                await roleManager.CreateAsync(new IdentityRole<int>("SuperAdmin"));

            if (!await roleManager.RoleExistsAsync(UserType.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole<int>(UserType.Admin.ToString()));

            List<Tuple<User, string>> users = new () 
            {
                new(
                    new()
                    {
                        FullName = "نوفكس مشرف",
                        Email = "info@novixcode.com",
                        EmailConfirmed = true,
                        UserCode = "AD-1",
                        UserName = "NovixCode",
                        CreatedAt = DateTime.UtcNow,
                        UserType = UserType.Admin.ToString()
                    },
                    "Novix@12345"
                ),
                new(
                    new()
                    {
                        FullName = "حوشي مشرف",
                        Email = "support@hoshi.ly",
                        EmailConfirmed = true,
                        UserCode = "AD-2",
                        UserName = "Hoshi",
                        CreatedAt = DateTime.UtcNow,
                        UserType = UserType.Admin.ToString()
                    },
                    "Hoshi@12345"
                )
            };

            foreach (var user in users)
                await CheckThenAddAdminUser(userManager, user);
        }
        public static async Task CheckThenAddAdminUser(UserManager<User> userManager, Tuple<User, string> user)
        {
            // Check if the admin user exists
            var adminUser = await userManager.FindByEmailAsync(user.Item1.Email!);
            if (adminUser == null)
            {
                var result = await userManager.CreateAsync(user.Item1, user.Item2);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user.Item1, ["SuperAdmin", UserType.Admin.ToString()]);
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

        //public static async Task SeedNotificationTypesAsync(IServiceProvider serviceProvider)
        //{
        //    using var scope = serviceProvider.CreateScope();
        //    var context = scope.ServiceProvider.GetRequiredService<HoshiDbContext>();

        //    var notificationTypes = new List<NotificationType>
        //    {
        //            // notifications
        //            new NotificationType { Title = "اشعار بإنشاء طلب", Type = "For_Worker", ForClient = false },
        //            new NotificationType { Title = "اشعار للاختبار", Type = "For_Client", ForClient = true },
   
        //    };

        //    foreach (var notif in notificationTypes)
        //    {
        //        bool exists = await context.NotificationTypes
        //            .AnyAsync(n => n.Type == notif.Type);

        //        if (!exists)
        //        {
        //            context.NotificationTypes.Add(notif);
        //        }
        //    }

        //    await context.SaveChangesAsync();
        //}

    }
}

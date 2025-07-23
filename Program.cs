
using GenericCRUDLibrary.GenericMiddlewares;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using Hoshi.Repositories.WorkerWalletService;
using Hoshi.Repositories.WorkerOfferService;
using Hoshi.Repositories.ClientOfferService;
using Hoshi.Repositories.OrderVisitService;
using Hoshi.Repositories.WorkerVisitService;
using Hoshi.Repositories.ClientVisitService;
using Hoshi.Repositories.ServiceService;
using Hoshi.Repositories.ClientHomeService;
using Hoshi.Repositories.OrderService;
using Hoshi.Repositories.WorkerOrderService;
using Hoshi.Repositories.ClientOrderService;
using Hoshi.Repositories.UserService;
using Hoshi.Repositories.AuthService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using Hoshi.Repositories.WorkerHomeService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.EmailServiceFold;
using Hoshi.Repositories.TokenServ;

namespace Hoshi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            }); ;

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("Worker", new OpenApiInfo
                {
                    Title = "Worker APIs",
                    Version = "1.0",
                    Description = "This APIs for Worker in this project."
                });
            });
            builder.Services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("Client", new OpenApiInfo
                {
                    Title = "Client APIs",
                    Version = "1.0",
                    Description = "This APIs for Client in this project."
                });
            });
            builder.Services.AddSwaggerGen(op =>
            {
                op.SwaggerDoc("Admin", new OpenApiInfo
                {
                    Title = "Admin APIs",
                    Version = "1.0",
                    Description = "This APIs for Admin in this project."
                });
            });

            builder.Services.AddSwaggerGen();

            // Initialize Db Context
            builder.Services.AddDbContext<HoshiDbContext>(
                contextBuilder => contextBuilder.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
            );

            builder.Services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<HoshiDbContext>();

            // Dependence Injection of Generic CRUD Library Services:

            // Inject Generic CRUD Service to be used correctly in controllers.
            builder.Services.AddTransient(
                typeof(IGenericCRUDService<,,,,>),
                typeof(GenericCRUDService<,,,,>)
            );

            // Inject Generic Filtered Search and Pagination Service to be used correctly in controllers.
            builder.Services.AddTransient(
                typeof(IGenericFSPService<,,>),
                typeof(GenericFSPService<,,>)
            );



            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMemoryCache();


            builder.Services.AddTransient(typeof(IAuthService), typeof(AuthService));

			builder.Services.AddTransient(typeof(IUserService), typeof(UserService));

			builder.Services.AddTransient(typeof(IClientOrderService), typeof(ClientOrderService));

			builder.Services.AddTransient(typeof(IWorkerOrderService), typeof(WorkerOrderService));

			builder.Services.AddTransient(typeof(IOrderService), typeof(OrderService));

			builder.Services.AddTransient(typeof(IClientHomeService), typeof(ClientHomeService));

			builder.Services.AddTransient(typeof(IServiceService), typeof(ServiceService));

			builder.Services.AddTransient(typeof(IClientVisitService), typeof(ClientVisitService));

			builder.Services.AddTransient(typeof(IWorkerVisitService), typeof(WorkerVisitService));

			builder.Services.AddTransient(typeof(IOrderVisitService), typeof(OrderVisitService));

			builder.Services.AddTransient(typeof(IClientOfferService), typeof(ClientOfferService));

			builder.Services.AddTransient(typeof(IWorkerOfferService), typeof(WorkerOfferService));

			builder.Services.AddTransient(typeof(IWorkerWalletService), typeof(WorkerWalletService));
          
			builder.Services.AddTransient(typeof(IWorkerHomeService), typeof(WorkerHomeService));
          
            builder.Services.AddTransient(typeof(IFileService), typeof(FileService));
          
			builder.Services.AddTransient(typeof(IEmailService), typeof(EmailService));
          
			builder.Services.AddTransient(typeof(ITokenService), typeof(TokenService));
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(op =>
                {
					op.SwaggerEndpoint("/swagger/Worker/swagger.json", "Worker APIs");

					op.SwaggerEndpoint("/swagger/Client/swagger.json", "Client APIs");

					op.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin APIs");

                    op.DocumentTitle = "Hoshi - Swagger";

                    // This options to make swagger more easy to use.
                    // Make all endpoints ready to use directly when it open, you don't need to press on "Try It Out" button any more.
                    op.EnableTryItOutByDefault();
                    // Make all scheme models closed
                    op.DefaultModelsExpandDepth(0);
                    // Make all Endpoints and Controllers Collapse
                    op.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                });
            //}

            app.UseHttpsRedirection();

            app.UseMiddleware<GenericExceptionMiddleware>();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true));

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedRolesAsync(services);
                await SeedAdminUserAsync(services);
            }

            app.Run();
        }
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

    }


}


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
using Hoshi.Repositories.EmailServiceFold;
using Hoshi.Repositories.FileServiceFold;
using Hoshi.Repositories.TokenService;
using Hoshi.Repositories.WorkerHomeService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hoshi.Data.IdentitySeeders;
using System.Text.Json.Serialization;
using Hoshi.Repositories.OrderImageService;
using Hoshi.Repositories.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http.HttpResults;

using Hoshi.Repositories.WorkerSpecificationService;
using Hoshi.Repositories.ClientSpecificationService;
using Hoshi.Repositories.WorkerPaymentHistroyService;
using Hoshi.Repositories.PromotionService;
using Hoshi.Repositories.ArchiveService;
using Hoshi.Repositories.NotificationService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(op =>
            {
                // Add this line to apply using default value for Login type.
                op.UseInlineDefinitionsForEnums();

                op.SwaggerDoc("Worker", new OpenApiInfo
                {
                    Title = "Worker APIs",
                    Version = "1.0",
                    Description = "This APIs for Worker in this project."
                });

                op.SwaggerDoc("Client", new OpenApiInfo
                {
                    Title = "Client APIs",
                    Version = "1.0",
                    Description = "This APIs for Client in this project."
                });

                op.SwaggerDoc("Admin", new OpenApiInfo
                {
                    Title = "Admin APIs",
                    Version = "1.0",
                    Description = "This APIs for Admin in this project."
                });
            });

            // Initialize Db Context
            builder.Services.AddDbContext<HoshiDbContext>(
                contextBuilder => contextBuilder.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"])
            );

            builder.Services.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<HoshiDbContext>();

            // Dependence Injection of Generic CRUD Library Services:

            // Inject Generic CRUD Service to be used correctly in controllers.
            var configuration = builder.Configuration;
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddTransient(
                typeof(IGenericCRUDService<,,,,>),
                typeof(GenericCRUDService<,,,,>)
            );

            // Inject Generic Filtered Search and Pagination Service to be used correctly in controllers.
            builder.Services.AddTransient(
                typeof(IGenericFSPService<,,>),
                typeof(GenericFSPService<,,>)
            );

            // Add Services Injections

			builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddMemoryCache();

            builder.Services.AddTransient(typeof(IAuthService), typeof(AuthService));

            builder.Services.AddTransient(typeof(IArchiveService), typeof(ArchiveService));

			builder.Services.AddTransient(typeof(IUserService), typeof(UserService));

			builder.Services.AddTransient(typeof(IClientOrderService), typeof(ClientOrderService));

			builder.Services.AddTransient(typeof(IWorkerOrderService), typeof(WorkerOrderService));

			builder.Services.AddTransient(typeof(IOrderService), typeof(OrderService));

			builder.Services.AddTransient(typeof(IClientHomeService), typeof(ClientHomeService));

			builder.Services.AddTransient(typeof(IClientSpecificationService), typeof(ClientSpecificationService));

			builder.Services.AddTransient(typeof(IServiceService), typeof(ServiceService));

			builder.Services.AddTransient(typeof(IClientVisitService), typeof(ClientVisitService));

			builder.Services.AddTransient(typeof(IWorkerVisitService), typeof(WorkerVisitService));

			builder.Services.AddTransient(typeof(IOrderVisitService), typeof(OrderVisitService));

			builder.Services.AddTransient(typeof(IClientOfferService), typeof(ClientOfferService));

			builder.Services.AddTransient(typeof(IWorkerOfferService), typeof(WorkerOfferService));

			builder.Services.AddTransient(typeof(IWorkerWalletService), typeof(WorkerWalletService));
          
			builder.Services.AddTransient(typeof(IWorkerHomeService), typeof(WorkerHomeService));

			builder.Services.AddTransient(typeof(IWorkerSpecificationService), typeof(WorkerSpecificationService));

			builder.Services.AddTransient(typeof(IWorkerPaymentHistroyService), typeof(WorkerPaymentHistroyService));

			builder.Services.AddTransient(typeof(IPromotionService), typeof(PromotionService));

			builder.Services.AddTransient(typeof(IOrderImageService), typeof(OrderImageService));
          
            builder.Services.AddTransient(typeof(IFileService), typeof(FileService));
          
			builder.Services.AddTransient(typeof(IEmailService), typeof(EmailService));
          
			builder.Services.AddTransient(typeof(ITokenService), typeof(TokenService));
            builder.Services.AddTransient(typeof(INotificationServiceHandler), typeof(NotificationServiceHandler));
            builder.Services.AddSignalR();
            
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
                await IdentitySeeder.SeedRolesAsync(services);
                await IdentitySeeder.SeedAdminUserAsync(services);
                await IdentitySeeder.SeedNotificationTypesAsync(services);
            }
            
           
            app.MapHub<NotificationHub>("/notification-hub");
            app.Run();
        }
    }
}

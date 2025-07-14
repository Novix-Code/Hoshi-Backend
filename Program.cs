
using GenericCRUDLibrary.GenericMiddlewares;
using GenericCRUDLibrary.GenericRepositories.GenericCRUDService;
using GenericCRUDLibrary.GenericRepositories.GenericFSPService;
using Hoshi.Data;
using Hoshi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Hoshi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(op =>
                {
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

            app.Run();
        }
    }
}

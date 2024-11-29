using Microsoft.OpenApi.Models;

namespace AngAspPnx.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddServer(new OpenApiServer
                {
                    Description = "Development Server",
                    Url = "https://localhost:7197"
                });

                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"] + e.ActionDescriptor.RouteValues["controller"]}");
            });



            var app = builder.Build();

            app.UseCors(builder => builder.WithOrigins("*"));

            app.UseSwagger().UseSwaggerUI();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}

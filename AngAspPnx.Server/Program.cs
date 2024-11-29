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
            });



            var app = builder.Build();

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

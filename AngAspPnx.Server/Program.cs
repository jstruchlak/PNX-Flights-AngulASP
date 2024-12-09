using Microsoft.OpenApi.Models;
using AngAspPnx.Server.Data;
using AngAspPnx.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AngAspPnx.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add DbContext
            builder.Services.AddDbContext<Entities>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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


            builder.Services.AddScoped<Entities>();

            var app = builder.Build();

            var entities = app.Services.CreateScope().ServiceProvider.GetService<Entities>();

            entities.Database.EnsureCreated();

            var random = new Random();

            if (!entities.Flights.Any())
            {
                Flight[] flightsToSeed = new Flight[]
            {
    new (   Guid.NewGuid(),
            "Pernix Airways",
            random.Next(90, 5000).ToString(),
            new TimePlace("Sydney", DateTime.Now.AddHours(random.Next(1, 3))),
            new TimePlace("Melbourne", DateTime.Now.AddHours(random.Next(4, 10))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Virgin Australia",
            random.Next(90, 5000).ToString(),
            new TimePlace("Brisbane", DateTime.Now.AddHours(random.Next(1, 10))),
            new TimePlace("Adelaide", DateTime.Now.AddHours(random.Next(4, 15))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Pernix Moon Landings",
            random.Next(90, 5000).ToString(),
            new TimePlace("Perth", DateTime.Now.AddHours(random.Next(1, 15))),
            new TimePlace("Darwin", DateTime.Now.AddHours(random.Next(4, 18))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Jet Star",
            random.Next(90, 5000).ToString(),
            new TimePlace("Hobart", DateTime.Now.AddHours(random.Next(1, 21))),
            new TimePlace("Canberra", DateTime.Now.AddHours(random.Next(4, 21))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Rex Airlines",
            random.Next(90, 5000).ToString(),
            new TimePlace("Adelaide", DateTime.Now.AddHours(random.Next(1, 23))),
            new TimePlace("Brisbane", DateTime.Now.AddHours(random.Next(4, 25))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Pernix Airline",
            random.Next(90, 5000).ToString(),
            new TimePlace("Melbourne", DateTime.Now.AddHours(random.Next(1, 15))),
            new TimePlace("Sydney", DateTime.Now.AddHours(random.Next(4, 19))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Qantas",
            random.Next(90, 5000).ToString(),
            new TimePlace("Darwin", DateTime.Now.AddHours(random.Next(1, 55))),
            new TimePlace("Perth", DateTime.Now.AddHours(random.Next(4, 58))),
            random.Next(1, 853)),
    new (   Guid.NewGuid(),
            "Virgin Australia",
            random.Next(90, 5000).ToString(),
            new TimePlace("Canberra", DateTime.Now.AddHours(random.Next(1, 58))),
            new TimePlace("Hobart", DateTime.Now.AddHours(random.Next(4, 60))),
            random.Next(1, 853))
};

                entities.Flights.AddRange(flightsToSeed);
                entities.SaveChanges();
            }

            app.UseCors(builder => builder
            .WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

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

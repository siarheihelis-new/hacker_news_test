
using Scalar.AspNetCore;
using HackerNews.Core.AspNet;
using HackerNews.Core.Caching;

namespace HackerNews.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration
               .AddJsonConfiguration()
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddObjectCache();
            builder.Services.AddApiModules();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseApiModules();
            app.MapControllers();

            app.Run();
        }
    }
}


using System.Threading.Tasks;
using Hangfire;

using Henry.Scheduling.Api.Middleware;
using Henry.Scheduling.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Henry.Scheduling.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Hangfire
            builder.Services.ConfigureHangfire(builder.Configuration);

            var config = builder.Configuration["ConnectionStrings:AppDataContext"];

            builder.Services.AddDbContext<AppDataContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(config,
                    b => b.MigrationsAssembly("Henry.Scheduling"));
            });

            await using var serviceProvider = builder.Services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<AppDataContext>();
            await context.Database.MigrateAsync();
            await context.Database.EnsureCreatedAsync();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHangfireDashboard("/dashboard", new DashboardOptions
                {
                    Authorization = new[] { new DashboardAuthFilter() }
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
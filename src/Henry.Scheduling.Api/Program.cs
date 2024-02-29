
using System;
using FluentValidation;

using Hangfire;

using Henry.Scheduling.Api.Infrastructure.Data;
using Henry.Scheduling.Api.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Reflection;
using System.Threading.Tasks;
using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data.Entities;

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

            // Add FluentValidation
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add MediatR
            builder.Services.AddMediatR(configuration =>
            {
                configuration.AutoRegisterRequestProcessors = true;
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Add Data Persistence
            builder.Services.AddDbContext<AppDataContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDataContext"));
            });

            // Apply Migrations
            await using var serviceProvider = builder.Services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<AppDataContext>();
            await context.Database.MigrateAsync();
            //await context.Database.EnsureCreatedAsync();

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
                await LoadDemoDataAsync(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }

        private static async Task LoadDemoDataAsync(AppDataContext dataContext)
        {
            // don't double up on the demo data
            if (await dataContext.Providers.AnyAsync())
                return;

            // create some Providers
            for (var i = 0; i < 20; i++)
            {
                await dataContext.Providers.AddAsync(new Provider()
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = Constants.UserIdSystem,
                    CreatedUtc = DateTime.UtcNow,
                    Name = $"Provider_{i}"
                });
            }

            // create some clients
            for (var i = 0; i < 20; i++)
            {
                await dataContext.Clients.AddAsync(new Client()
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = Constants.UserIdSystem,
                    CreatedUtc = DateTime.UtcNow,
                    Name = $"Client_{i}"
                });
            }

            // create some slots for the providers

            // create some appointments
        }
    }
}
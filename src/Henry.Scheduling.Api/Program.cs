
using FluentValidation;

using Hangfire;

using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data;
using Henry.Scheduling.Api.Infrastructure.Data.Entities;
using Henry.Scheduling.Api.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;

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

            // Add services
            builder.Services.ConfigureServices();

            // Add Hangfire
            builder.Services.ConfigureHangfire(builder.Configuration);

            // Add FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add AutoMapper
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Add Data Persistence
            builder.Services.AddDbContext<AppDataContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDataContext"));
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(s => s.FullName.Replace("+", "."));
            });

            // Apply Migrations
            await using var serviceProvider = builder.Services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<AppDataContext>();
            await context.Database.MigrateAsync();
            //await context.Database.EnsureCreatedAsync();
            serviceProvider.ConfigureHangfireJobs();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(x =>
                {
                    x.SerializeAsV2 = true;
                });
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
                var provider = new Provider()
                {
                    Id = Guid.NewGuid(),
                    CreatedBy = Constants.UserIdSystem,
                    CreatedUtc = DateTime.UtcNow,
                    Name = $"Provider_{i}"
                };

                // create some slots for the provider
                for (var j = 0; j < 20; j++)
                {
                    provider.Slots.Add(new Slot()
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = Constants.UserIdSystem,
                        CreatedUtc = DateTime.UtcNow,
                        StartUtc = DateTime.UtcNow.AddDays(j + 2),
                        EndUtc = DateTime.UtcNow.AddMinutes(j * 15),
                        ProviderId = provider.Id
                    });
                }
                await dataContext.Providers.AddAsync(provider);
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

            // create some appointments

            await dataContext.SaveChangesAsync();
        }
    }
}
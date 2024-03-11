
using FluentValidation;
using FluentValidation.AspNetCore;

using Hangfire;

using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data;
using Henry.Scheduling.Api.Infrastructure.Data.Entities;
using Henry.Scheduling.Api.Middleware;

using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using StackExchange.Redis;

using System;
using System.Reflection;
using System.Threading.Tasks;
using StackExchange.Redis.Configuration;

namespace Henry.Scheduling.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var hostAssembly = Assembly.GetExecutingAssembly();

            builder.Services.AddControllers();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add Serilog
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            // Add services
            builder.Services.ConfigureServices();

            // Add Hangfire
            builder.Services.ConfigureHangfire(builder.Configuration);

            // Add FluentValidation
            builder.Services.AddFluentValidation([hostAssembly]);
            builder.Services.AddValidatorsFromAssembly(hostAssembly);

            // Add AutoMapper
            builder.Services.AddAutoMapper(hostAssembly);

            // Add MediatR
            builder.Services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssembly(hostAssembly))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(CorrelationIdBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(Middleware.ValidationBehavior<,>));

            // Add Data Persistence
            builder.Services.AddDbContext<AppDataContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDataContext"));
            });

            // Add Caching
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "HSA_"; // Henry.Scheduling.Api acronym (only one app using; good practice)
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(s => s.FullName.Replace("+", "."));
            });

            // Apply Migrations
            await using var serviceProvider = builder.Services.BuildServiceProvider();
            var context = serviceProvider.GetRequiredService<AppDataContext>();
            await context.Database.MigrateAsync();

            serviceProvider.ConfigureHangfireJobs();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.HeadContent = "<a href=\"https://localhost:5001/dashboard\" target=\"_blank\">Hangfire Dashboard</a></br><a href=\"http://localhost:8081/#/events?range=1d\" target=\"_blank\">Seq</a>";
                });
                app.UseHangfireDashboard("/dashboard", new DashboardOptions
                {
                    Authorization = new[] { new DashboardAuthFilter() }
                });
                await LoadDemoDataAsync(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseSerilogRequestLogging();

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
                        StartUtc = DateTime.UtcNow.AddDays(j),
                        EndUtc = DateTime.UtcNow.AddDays(j).AddMinutes(j * 15),
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
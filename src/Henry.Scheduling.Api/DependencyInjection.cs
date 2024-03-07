using Hangfire;

using Henry.Scheduling.Api.Application.Appointment.Commands;
using Henry.Scheduling.Api.Application.Slot.Commands;
using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Jobs;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Henry.Scheduling.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // services
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IGenerateSlots, SlotGenerator>();

            // hangfire job
            services.AddScoped<IExpireReservations, ReservationExpiryJob>();

            // hangfire job worker
            services.AddScoped<IExpireAnAppointment, ExpireAppointment>();

            return services;
        }

        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("Hangfire")));
            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.WorkerCount = Environment.ProcessorCount;
            });
            return services;
        }

        public static IServiceProvider ConfigureHangfireJobs(this IServiceProvider services)
        {
            // TODO: Do I really need to get a service scope here?
            var serviceScope = services.CreateScope();
            var recurringJobManager = serviceScope.ServiceProvider.GetService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<IExpireReservations>(nameof(ReservationExpiryJob),
                job => job.ExecuteAsync(), "* * * * *");

            return services;
        }
    }
}
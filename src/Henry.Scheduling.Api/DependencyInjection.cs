using Hangfire;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Henry.Scheduling.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration config)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(config.GetConnectionString("Hangfire")));
            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.WorkerCount = Environment.ProcessorCount;
            });
            return services;
        }
    }
}
using Hangfire;

using Henry.Scheduling.Api.Application.Appointment.Commands;
using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data;
using Henry.Scheduling.Api.Infrastructure.Jobs.Contracts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Linq;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Infrastructure.Jobs
{
    public interface IExpireReservations : IAmARecurringJob { }

    public class ReservationExpiryJob : IExpireReservations
    {
        private readonly ILogger<ReservationExpiryJob> _logger;

        private readonly AppDataContext _dataContext;

        private readonly IDateTimeProvider _dateTimeProvider;

        public ReservationExpiryJob(
            ILogger<ReservationExpiryJob> logger,
            AppDataContext dataContext,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _dataContext = dataContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation($"Started {nameof(ReservationExpiryJob)}");

            // get expired appointments
            var expiredAppointments = await _dataContext
                .Appointments
                .Where(a => a.CreatedUtc.AddMinutes(30) > _dateTimeProvider.UtcNow())
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation("Found {expiredReservationsCount} reservations to expire", expiredAppointments.Count);

            // send them off for cancellation and downstream tasks
            expiredAppointments.ForEach(x =>
            {
                BackgroundJob.Enqueue<IExpireAnAppointment>(job =>
                    job.ProcessAsync(x.Id));
            });
        }
    }
}
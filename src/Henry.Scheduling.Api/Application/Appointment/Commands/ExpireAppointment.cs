using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Appointment.Commands
{
    public interface IExpireAnAppointment
    {
        public Task ProcessAsync(Guid appointmentId);
    }

    public class ExpireAppointment : IExpireAnAppointment
    {
        private readonly ILogger<ExpireAppointment> _logger;
        private readonly AppDataContext _dataContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ExpireAppointment(
            ILogger<ExpireAppointment> logger,
            AppDataContext dataContext,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _dataContext = dataContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task ProcessAsync(Guid appointmentId)
        {
            _logger.LogInformation("Expiring appointment with id:", appointmentId);

            var appointment = await _dataContext
                .Appointments
                .FirstOrDefaultAsync(x => x.Id == appointmentId);

            if (appointment == null)
            {
                throw new ArgumentException($"Invalid appointmentId: {appointmentId}");
            }

            appointment.ExpiredUtc = _dateTimeProvider.UtcNow();

            // TODO: Raise a domain event here - downstream might want to release the slot

            // TODO: Raise integration event here - some other service will likely want to send
            // a notification to the client that the reservation was not confirmed
            // and that the appointment was expired

            await _dataContext.SaveChangesAsync();
        }
    }
}

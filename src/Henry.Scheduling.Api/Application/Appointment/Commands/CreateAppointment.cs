using FluentValidation;

using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Common.Commands;
using Henry.Scheduling.Api.Common.Exceptions;
using Henry.Scheduling.Api.Infrastructure.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Appointment.Commands
{
    public class CreateAppointment
    {
        public class Command : TrackableCommand<Dto>
        {
            public Guid ClientId { get; set; }
            public Guid SlotId { get; set; }
        }

        public class Dto
        {
            public Guid ClientId { get; set; }

            public Guid SlotId { get; set; }

            public Guid AppointmentId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.ClientId).NotEmpty();
                RuleFor(x => x.SlotId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            private readonly ILogger<CreateAppointment> _logger;
            private readonly AppDataContext _dataContext;
            private readonly IDateTimeProvider _dateTimeProvider;

            public Handler(
                ILogger<CreateAppointment> logger,
                AppDataContext dataContext,
                IDateTimeProvider dateTimeProvider)
            {
                _logger = logger;
                _dataContext = dataContext;
                _dateTimeProvider = dateTimeProvider;
            }

            public async Task<Dto> Handle(Command command, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Handler began with: {@command}", command);

                var slot = await _dataContext.Slots
                    .FirstOrDefaultAsync(s => s.Id == command.SlotId, cancellationToken);

                if (slot == null)
                {
                    _logger.LogError("Invalid slotId: {slotId}", command.SlotId);
                    throw new InvalidCommandException($"Invalid slotId: {command.SlotId}");
                }

                if (slot.AppointmentId.HasValue)
                {
                    // TODO: This could probably be more explicit
                    // i.e. if the appointmentId is one that belongs to the current client,
                    // we might want to provide a more specific message (i.e. you already booked this slot)
                    _logger.LogError("Slot is no longer available: {slotId}", command.SlotId);
                    throw new InvalidCommandException("Slot is no longer available");
                }

                if (slot.StartUtc < _dateTimeProvider.UtcNow().AddHours(24))
                {
                    _logger.LogError("Slot is less than 24 hours from now. {slotId}", command.SlotId);
                    throw new InvalidCommandException("Slot is less than 24 hours from now");
                }

                var appointment = new Infrastructure.Data.Entities.Appointment()
                {
                    ClientId = command.ClientId,
                    CreatedBy = command.ClientId,
                    CreatedUtc = _dateTimeProvider.UtcNow(),
                    SlotId = command.SlotId,
                    ProviderId = slot.ProviderId,
                    CorrelationId = command.CorrelationId
                };

                await _dataContext.Appointments.AddAsync(appointment, cancellationToken);

                slot.AppointmentId = appointment.Id;

                // TODO: Raise integration event (AppointmentCreated); downstream service will likely send confirmation email to client

                await _dataContext.SaveChangesAsync(cancellationToken);

                return new Dto()
                {
                    ClientId = command.ClientId,
                    SlotId = command.SlotId,
                    AppointmentId = appointment.Id
                };
            }
        }
    }
}

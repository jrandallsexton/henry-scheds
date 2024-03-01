﻿using FluentValidation;

using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data;

using MediatR;

using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;
using Henry.Scheduling.Api.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Henry.Scheduling.Api.Application.Appointment.Commands
{
    public class ConfirmAppointment
    {
        public class Command : IRequest<Dto>
        {
            public Guid ClientId { get; set; }
            public Guid AppointmentId { get; set; }
        }

        public class Dto
        {
            public Guid AppointmentId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.ClientId).NotEmpty();
                RuleFor(x => x.AppointmentId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            private readonly ILogger<ConfirmAppointment> _logger;
            private readonly IDateTimeProvider _dateTimeProvider;
            private readonly AppDataContext _dataContext;

            public Handler(
                ILogger<ConfirmAppointment> logger,
                IDateTimeProvider dateTimeProvider,
                AppDataContext dataContext)
            {
                _logger = logger;
                _dateTimeProvider = dateTimeProvider;
                _dataContext = dataContext;
            }

            public async Task<Dto> Handle(Command command, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Handler began with:", command);

                var appointment = await _dataContext
                    .Appointments
                    .FirstOrDefaultAsync(x => x.Id == command.AppointmentId && x.ClientId == command.ClientId,
                        cancellationToken);

                if (appointment == null)
                {
                    throw new ResourceNotFoundException("Invalid appointment");
                }

                appointment.ConfirmedUtc = _dateTimeProvider.UtcNow();

                return new Dto()
                {
                    AppointmentId = command.AppointmentId
                };
            }
        }
    }
}

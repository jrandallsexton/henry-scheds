using FluentValidation;

using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Henry.Scheduling.Api.Application.Slot.Commands
{
    public class CreateSlotsForProvider
    {
        public class Command : IRequest<Dto>
        {
            public Guid ProviderId { get; set; }
            public DateTime StartUtc { get; set; }
            public DateTime EndUtc { get; set; }
        }

        public class Dto
        {
            public Guid ProviderId { get; set; }
            public int SlotCreationCount { get; set; }
            public List<Guid> SlotIds { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.StartUtc).NotEmpty();
                RuleFor(x => x.EndUtc).NotEmpty();
                // TODO: Rule to ensure EndUtc > StartUtc
                // TODO: Rule to ensure that EndUtc is at least 15 min greater than StartUtc (would create a single slot)
                // TODO: Rule to ensure StartUtc is 00/15/30/45
            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            private readonly ILogger<CreateSlotsForProvider> _logger;
            private readonly AppDataContext _dataContext;
            private readonly IDateTimeProvider _dateTimeProvider;

            public Handler(
                ILogger<CreateSlotsForProvider> logger,
                AppDataContext dataContext,
                IDateTimeProvider dateTimeProvider)
            {
                _logger = logger;
                _dataContext = dataContext;
                _dateTimeProvider = dateTimeProvider;
            }

            public async Task<Dto> Handle(Command command, CancellationToken cancellationToken)
            {
                var provider = await _dataContext
                    .Providers
                    .Include(p => p.Slots)
                    .FirstOrDefaultAsync(p => p.Id == command.ProviderId, cancellationToken: cancellationToken);

                if (provider == null)
                {
                    _logger.LogError("Invalid providerId", command.ProviderId);
                    throw new ArgumentException($"Invalid providerId: {command.ProviderId}");
                }

                // TODO: What if slots already exist for the period defined by the command?
                
                // get the duration between start and end
                var duration = command.EndUtc - command.StartUtc;

                var numberOfSlots = Math.Floor(duration.TotalMinutes / 15);

                var slotCount = 0;
                var startTime = command.StartUtc;
                while (startTime < command.EndUtc)
                {
                    provider.Slots.Add(new Infrastructure.Data.Entities.Slot()
                    {
                        StartUtc = command.StartUtc,
                        EndUtc = command.StartUtc.AddMinutes(15),
                        CreatedBy = command.ProviderId,
                        CreatedUtc = _dateTimeProvider.UtcNow()
                    });

                    startTime = startTime.AddMinutes(15);
                    slotCount++;
                }

                await _dataContext.SaveChangesAsync(cancellationToken);

                // break it into timeslots

                // save them

                return new Dto()
                {
                    ProviderId = command.ProviderId,
                    SlotCreationCount = slotCount,
                    SlotIds = new List<Guid>()
                };
            }
        }
    }
}

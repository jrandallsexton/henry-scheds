﻿using FluentValidation;

using Henry.Scheduling.Api.Common.Commands;
using Henry.Scheduling.Api.Common.Exceptions;
using Henry.Scheduling.Api.Infrastructure.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Slot.Commands
{
    public class CreateSlotsForProvider
    {
        public class Command : TrackableCommand<Dto>
        {
            public Guid ProviderId { get; set; }
            public DateTime StartUtc { get; set; }
            public DateTime EndUtc { get; set; }
        }

        public class Dto
        {
            public Guid ProviderId { get; set; }
            public List<Guid> SlotIds { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.StartUtc).NotNull().NotEmpty()
                    .WithMessage("StartUtc cannot be empty or null");
                RuleFor(x => x.EndUtc).NotNull().NotEmpty()
                    .WithMessage("EndUtc cannot be empty or null");
                RuleFor(x => x.EndUtc).GreaterThan(x => x.StartUtc.AddMinutes(15))
                    .WithMessage("EndUtc must be at least 15 minutes past StartUtc");
                RuleFor(x => x.StartUtc.Minute).Must(x => x is 0 or 15 or 30 or 45)
                    .WithMessage("StartUtc minutes must be one of: 00, 15, 30, or 45");
                RuleFor(x => x.EndUtc.Minute).Must(x => x is 0 or 15 or 30 or 45)
                    .WithMessage("EndUtc minutes must be one of: 00, 15, 30, or 45");
            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            private readonly ILogger<CreateSlotsForProvider> _logger;
            private readonly AppDataContext _dataContext;
            private readonly IGenerateSlots _slotGenerator;
            private const int SlotDurationInMinutes = 15;

            public Handler(
                ILogger<CreateSlotsForProvider> logger,
                AppDataContext dataContext,
                IGenerateSlots slotGenerator)
            {
                _logger = logger;
                _dataContext = dataContext;
                _slotGenerator = slotGenerator;
            }

            public async Task<Dto> Handle(Command command, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Handler began with {@command}", command);

                var provider = await _dataContext
                    .Providers
                    .Include(p => p.Slots)
                    .FirstOrDefaultAsync(p => p.Id == command.ProviderId, cancellationToken: cancellationToken);

                if (provider == null)
                {
                    _logger.LogError("Invalid providerId: {providerId}", command.ProviderId);
                    throw new ResourceNotFoundException($"Invalid providerId: {command.ProviderId}");
                }

                var slots = _slotGenerator.GenerateSlots(
                    SlotDurationInMinutes, command, provider.Slots);

                provider.Slots.AddRange(slots);

                // TODO: Raise domain/integration event here (ProviderScheduleCreated) for downstream consumers

                await _dataContext.SaveChangesAsync(cancellationToken);

                return new Dto()
                {
                    ProviderId = command.ProviderId,
                    SlotIds = provider.Slots
                        .Where(x => x.CorrelationId == command.CorrelationId)
                        .Select(y => y.Id)
                        .ToList()
                };
            }
        }
    }
}

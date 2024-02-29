using FluentValidation;

using MediatR;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            public async Task<Dto> Handle(Command command, CancellationToken cancellationToken)
            {
                // TODO: What if slots already exist for the period defined by the command?
                
                // get the duration between start and end
                var duration = command.EndUtc - command.StartUtc;

                var numberOfSlots = Math.Floor(duration.TotalMinutes / 15);

                // break it into timeslots

                // save them

                return new Dto();
            }
        }
    }
}

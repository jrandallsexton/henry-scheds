using MediatR;

using System;

namespace Henry.Scheduling.Api.Common.Commands
{
    public abstract class TrackableCommand<T> : IRequest<T>
    {
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
    }
}

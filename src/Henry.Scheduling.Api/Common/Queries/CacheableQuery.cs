using MediatR;

namespace Henry.Scheduling.Api.Common.Queries
{
    public class CacheableQuery<T> : IRequest<T>
    {
        public bool BypassCache { get; set; }
    }
}
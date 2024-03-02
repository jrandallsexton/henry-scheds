using Henry.Scheduling.Api.Common.Commands;

using MediatR;

using Microsoft.AspNetCore.Http;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Middleware
{
    public sealed class CorrelationIdBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : TrackableCommand<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdBehavior(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var correlationId = _httpContextAccessor.HttpContext.Request.Headers["X-Correlation-Id"];
                if (!string.IsNullOrEmpty(correlationId))
                {
                    request.CorrelationId = Guid.TryParse(correlationId, out var result) ?
                        result :
                        Guid.NewGuid();
                }
                else
                {
                    request.CorrelationId = Guid.NewGuid();
                }
            }
            else
            {
                request.CorrelationId = Guid.NewGuid();
            }
            return await next();
        }
    }
}

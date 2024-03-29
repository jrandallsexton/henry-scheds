﻿using Henry.Scheduling.Api.Common.Queries;
using Henry.Scheduling.Api.Extensions;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Middleware
{
    public sealed class QueryCachingBehavior<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : CacheableQuery<TResponse> where TResponse : class
    {
        private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;
        private readonly IDistributedCache _cache;

        public QueryCachingBehavior(
            ILogger<QueryCachingBehavior<TRequest, TResponse>> logger,
            IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            TResponse response;
            if (request.BypassCache) return await next();

            var cacheKey = request.GetType().FullName;

            var cachedResponse = await _cache.GetRecordAsync<TResponse>(cacheKey);

            if (cachedResponse != null)
            {
                _logger.LogInformation("Fetched from Cache: {cacheKey}", cacheKey);
            }
            else
            {
                cachedResponse = await GetResponseAndAddToCache();
                _logger.LogInformation("Added to Cache: {cacheKey}", cacheKey);
            }
            return cachedResponse;

            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();
                await _cache.SetRecordAsync(cacheKey, response);
                return response;
            }
        }
    }
}

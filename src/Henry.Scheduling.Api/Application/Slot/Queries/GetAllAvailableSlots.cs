using AutoMapper;

using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Common.Mapping;
using Henry.Scheduling.Api.Infrastructure.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Henry.Scheduling.Api.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Henry.Scheduling.Api.Application.Slot.Queries
{
    public class GetAllAvailableSlots
    {
        public class Query : IRequest<List<Dto>> { }

        public class Dto : IMapFrom<Infrastructure.Data.Entities.Slot>
        {
            public Guid Id { get; set; }

            public Guid ProviderId { get; set; }

            public DateTime StartUtc { get; set; }

            public DateTime EndUtc { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<Infrastructure.Data.Entities.Slot, Dto>();
            }
        }

        public class Handler : IRequestHandler<Query, List<Dto>>
        {
            private readonly AppDataContext _dataContext;
            private readonly IDateTimeProvider _dateTimeProvider;
            private readonly IMapper _mapper;
            private readonly IDistributedCache _cache;

            public Handler(
                AppDataContext dataContext,
                IDateTimeProvider dateTimeProvider,
                IMapper mapper,
                IDistributedCache cache)
            {
                _dataContext = dataContext;
                _dateTimeProvider = dateTimeProvider;
                _mapper = mapper;
                _cache = cache;
            }

            public async Task<List<Dto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cachedSlots = await _cache.GetRecordAsync<List<Dto>>(nameof(GetAllAvailableSlots));
                if (cachedSlots != null)
                {
                    return cachedSlots;
                }

                var slots = await _dataContext
                    .Slots
                    .Where(x => x.AppointmentId == null && x.StartUtc >= _dateTimeProvider.UtcNow().AddHours(24))
                    .OrderBy(x => x.StartUtc)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                //return _mapper.Map<List<Dto>>(slots);

                cachedSlots = _mapper.Map<List<Dto>>(slots);
                await _cache.SetRecordAsync(nameof(GetAllAvailableSlots), cachedSlots);
                return cachedSlots;
            }
        }
    }
}
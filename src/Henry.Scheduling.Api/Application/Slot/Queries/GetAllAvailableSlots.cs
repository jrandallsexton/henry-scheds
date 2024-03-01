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

            public Handler(
                AppDataContext dataContext,
                IDateTimeProvider dateTimeProvider,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _dateTimeProvider = dateTimeProvider;
                _mapper = mapper;
            }

            public async Task<List<Dto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var slots = await _dataContext
                    .Slots
                    .Where(x => x.AppointmentId == null && x.StartUtc >= _dateTimeProvider.UtcNow().AddHours(24))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<Dto>>(slots);
            }
        }
    }
}
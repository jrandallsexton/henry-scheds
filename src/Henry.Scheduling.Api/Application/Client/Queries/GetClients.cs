using AutoMapper;

using Henry.Scheduling.Api.Common.Mapping;
using Henry.Scheduling.Api.Infrastructure.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Client.Queries
{
    public class GetClients
    {
        public class Query : IRequest<List<Dto>> { }

        public class Dto() : IMapFrom<Infrastructure.Data.Entities.Client>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }

            public void Mapping(Profile profile)
            {
                profile.CreateMap<Infrastructure.Data.Entities.Client, Dto>();
            }
        }

        public class Handler : IRequestHandler<Query, List<Dto>>
        {
            private readonly AppDataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(
                AppDataContext dataContext,
                IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<List<Dto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var clients = await _dataContext
                    .Clients
                    .AsNoTracking()
                    .OrderBy(x => x.Name)
                    .ToListAsync(cancellationToken);

                return _mapper.Map<List<Dto>>(clients);
            }
        }
    }
}
using Henry.Scheduling.Api.Application.Client.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Client
{
    [Route("api/clients")]
    public class ClientController
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetClients.Dto>>> GetAllAvailableSlots()
        {
            return await _mediator.Send(new GetClients.Query());
        }
    }
}

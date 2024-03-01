using Henry.Scheduling.Api.Application.Provider.Queries;
using Henry.Scheduling.Api.Application.Slot.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Provider
{
    [Route("api/providers")]
    public class ProviderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProviderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{id}/schedule")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreateSlotsForProvider.Dto>> CreateSlotsFromSchedule(
            [FromBody] CreateSlotsForProvider.Command command)
        {
            var result = await _mediator.Send(command);
            return Created(nameof(CreateSlotsFromSchedule), result);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllProviders.Dto>>> GetAllProviders()
        {
            return await _mediator.Send(new GetAllProviders.Query());
        }
    }
}
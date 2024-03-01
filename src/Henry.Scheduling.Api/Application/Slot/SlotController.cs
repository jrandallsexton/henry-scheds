using Henry.Scheduling.Api.Application.Slot.Commands;
using Henry.Scheduling.Api.Application.Slot.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Slot
{
    [Route("api/slots")]
    public class SlotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SlotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CreateSlotsForProvider.Dto>> CreateSlotsForProvider(
            [FromBody] CreateSlotsForProvider.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllAvailableSlots.Dto>>> GetAllAvailableSlots()
        {
            return await _mediator.Send(new GetAllAvailableSlots.Query());
        }
    }
}

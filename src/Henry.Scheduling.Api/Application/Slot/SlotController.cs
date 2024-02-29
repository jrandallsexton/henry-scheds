using Henry.Scheduling.Api.Application.Slot.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Slot
{
    [Route("api/slot")]
    public class SlotController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SlotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<CreateSlotsForProvider.Dto> CreateSlotsForProvider(
            [FromBody] CreateSlotsForProvider.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}

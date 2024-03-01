using Henry.Scheduling.Api.Application.Appointment.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Appointment
{
    [Route("api/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CreateAppointment.Dto>> CreateAppointment(
            [FromBody] CreateAppointment.Command command)
        {
            var result = await _mediator.Send(command);
            return Created(nameof(CreateAppointment), result);
        }

        [HttpPut("{id}/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ConfirmAppointment.Dto>> ConfirmAppointment(Guid id,
            [FromBody]ConfirmAppointment.Command command)
        {
            // TODO: In real-life, we would (likely) take the ClientId from context
            // and remove the need for the body
            // As a result, this looks NOTHING like it would for real
            var tempCommand = new ConfirmAppointment.Command()
            {
                AppointmentId = id,
                ClientId = command.ClientId
            };
            return await _mediator.Send(tempCommand);
        }
    }
}
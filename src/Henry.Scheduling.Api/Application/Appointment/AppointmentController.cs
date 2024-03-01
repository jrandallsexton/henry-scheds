using System;
using Henry.Scheduling.Api.Application.Appointment.Commands;

using MediatR;

using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<ConfirmAppointment.Dto>> ConfirmAppointment(Guid id)
        {
            var command = new ConfirmAppointment.Command()
            {
                AppointmentId = id,
                ClientId = Guid.NewGuid()
            };

            return await _mediator.Send(command);
        }
    }
}
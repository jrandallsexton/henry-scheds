using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace Henry.Scheduling.Api.Application.Appointment.Commands
{
    public class ConfirmAppointment
    {
        public class Command : IRequest<Dto>
        {

        }

        public class Dto
        {

        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            public Task<Dto> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

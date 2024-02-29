using MediatR;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace Henry.Scheduling.Api.Application.Client.Commands
{
    public class CreateClient
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

using FluentValidation;

using Henry.Scheduling.Api.Common.Commands;
using Henry.Scheduling.Api.Infrastructure.Data;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Henry.Scheduling.Api.Application.Provider.Commands
{
    public class CreateProvider
    {
        public class Command : TrackableCommand<Dto>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class Dto
        {
            public Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Dto>
        {
            private readonly AppDataContext _dbContext;
            private readonly ILogger<CreateProvider> _logger;
            private readonly Validator _validator;

            public Handler(AppDataContext dataContext, ILogger<CreateProvider> logger)
            {
                _dbContext = dataContext;
                _logger = logger;
                _validator = new Validator();
            }

            public async Task<Dto> Handle(Command command, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Handler began with command: {command}", command);

                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                var providerExists = await _dbContext
                    .Providers
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == command.Id, cancellationToken);

                if (providerExists)
                {
                    var message = $"Provider already exists for Id: {command.Id}";
                    _logger.LogError(message);
                    throw new InvalidOperationException(message);
                }

                var provider = Map(command);
                await _dbContext.Providers.AddAsync(provider, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new Dto()
                {
                    Id = command.Id
                };

            }

            private static Infrastructure.Data.Entities.Provider Map(Command command)
            {
                return new Infrastructure.Data.Entities.Provider()
                {
                    Id = command.Id,
                    Name = command.Name,
                    CorrelationId = command.CorrelationId
                };
            }
        }
    }
}
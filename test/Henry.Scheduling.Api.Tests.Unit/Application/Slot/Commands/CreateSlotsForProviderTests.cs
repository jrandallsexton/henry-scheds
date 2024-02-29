using FluentValidation.TestHelper;

using Henry.Scheduling.Api.Application.Slot.Commands;

using System;
using System.Threading.Tasks;

using Xunit;

namespace Henry.Scheduling.Api.Tests.Unit.Application.Slot.Commands
{
    public class CreateSlotsForProviderTests
    {
        private readonly CreateSlotsForProvider.Validator _validator = new();

        [Fact]
        public async Task Missing_StartTime_FailsValidation()
        {
            // arrange
            var command = new CreateSlotsForProvider.Command()
            {
                EndUtc = DateTime.UtcNow
            };

            // act
            var result = await _validator.TestValidateAsync(command);

            // assert
            result.ShouldHaveValidationErrorFor(x => x.StartUtc);
        }
    }
}
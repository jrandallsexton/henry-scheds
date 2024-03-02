using FluentAssertions;

using FluentValidation.TestHelper;

using Henry.Scheduling.Api.Application.Slot.Commands;
using Henry.Scheduling.Api.Infrastructure.Data.Entities;

using System;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace Henry.Scheduling.Api.Tests.Unit.Application.Slot.Commands
{
    public class CreateSlotsForProviderTests : UnitTestBase<CreateSlotsForProvider>
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

        [Fact]
        public async Task Missing_EndTime_FailsValidation()
        {
            // arrange
            var command = new CreateSlotsForProvider.Command()
            {
                StartUtc = DateTime.UtcNow
            };

            // act
            var result = await _validator.TestValidateAsync(command);

            // assert
            result.ShouldHaveValidationErrorFor(x => x.EndUtc);
        }

        [Fact]
        public async Task StartTimeIsGreaterThanEndTime_FailsValidation()
        {
            // arrange
            var command = new CreateSlotsForProvider.Command()
            {
                StartUtc = DateTime.UtcNow,
                EndUtc = DateTime.UtcNow.AddHours(-1)
            };

            // act
            var result = await _validator.TestValidateAsync(command);

            // assert
            result.ShouldHaveValidationErrorFor(x => x.EndUtc);
        }

        [Fact]
        public async Task StartTimeMinuteIsNotValid_FailsValidation()
        {
            // arrange
            var command = new CreateSlotsForProvider.Command()
            {
                StartUtc = new DateTime(2024, 02, 29, 8, 1, 0),
                EndUtc = new DateTime(2024, 02, 29, 8, 0, 0).AddHours(8)
            };

            // act
            var result = await _validator.TestValidateAsync(command);

            // assert
            result.ShouldHaveValidationErrorFor(x => x.StartUtc.Minute);
        }

        [Fact]
        public async Task EndTimeMinuteIsNotValid_FailsValidation()
        {
            // arrange
            var command = new CreateSlotsForProvider.Command()
            {
                StartUtc = new DateTime(2024, 02, 29, 8, 0, 0),
                EndUtc = new DateTime(2024, 02, 29, 8, 1, 0).AddHours(8)
            };

            // act
            var result = await _validator.TestValidateAsync(command);

            // assert
            result.ShouldHaveValidationErrorFor(x => x.EndUtc.Minute);
        }

        /// <summary>
        /// This test class should be using inline data to cover all edge cases
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ValidCommand_CreatesSlots()
        {
            // arrange
            var providerId = Guid.NewGuid();
            await base.DataContext.Providers.AddAsync(new Provider()
            {
                Id = providerId,
                Name = "foo"
            });
            await base.DataContext.SaveChangesAsync();

            var command = new CreateSlotsForProvider.Command()
            {
                ProviderId = providerId,
                StartUtc = new DateTime(2024, 02, 29, 8, 0, 0),
                EndUtc = new DateTime(2024, 02, 29, 8, 0, 0).AddHours(8)
            };

            var handler = base.Mocker.CreateInstance<CreateSlotsForProvider.Handler>();

            // act
            var result = await handler.Handle(command, CancellationToken.None);

            // assert
            result.SlotIds.Count.Should().Be(32);
        }
    }
}
using BenchmarkDotNet.Attributes;

using Henry.Scheduling.Api.Application.Slot.Commands;
using Henry.Scheduling.Api.Common;
using Henry.Scheduling.Api.Infrastructure.Data.Entities;

namespace Henry.Scheduling.Api.Util.Perf.Application
{
    [MemoryDiagnoser]
    public class SlotGeneratorBenchmarker
    {
        private readonly int generatedSlotDurationInMinutes = 30;
        private CreateSlotsForProvider.Command _command;
        private List<Infrastructure.Data.Entities.Slot>? _existingSlots;

        [GlobalSetup]
        public void Setup()
        {
            _command = new CreateSlotsForProvider.Command()
            {
                ProviderId = Guid.NewGuid(),
                StartUtc = new DateTime(2024, 02, 29, 8, 0, 0),
                EndUtc = new DateTime(2024, 02, 29, 8, 0, 0).AddHours(8),
                CorrelationId = Guid.NewGuid()
            };

            _existingSlots = new List<Slot>();
        }

        [Benchmark]
        public new List<Infrastructure.Data.Entities.Slot> GenerateSlots()
        {
            var slotGenerator = new SlotGenerator(new DateTimeProvider());
            return slotGenerator.GenerateSlots(generatedSlotDurationInMinutes, _command, _existingSlots);
        }

        [Benchmark]
        public new List<Infrastructure.Data.Entities.Slot> GenerateSlotsNew()
        {
            var slotGenerator = new NewSlotGenerator(new DateTimeProvider());
            return slotGenerator.GenerateSlots(generatedSlotDurationInMinutes, _command, _existingSlots);
        }
    }

    public class NewSlotGenerator : IGenerateSlots
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public NewSlotGenerator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public List<Slot> GenerateSlots(int generatedSlotDurationInMinutes, CreateSlotsForProvider.Command command, List<Slot>? existingSlots)
        {
            var startTime = command.StartUtc;
            var endTime = command.EndUtc;
            var slots = new List<Infrastructure.Data.Entities.Slot>();

            if (existingSlots == null || existingSlots.Count == 0)
            {
                while (startTime < endTime)
                {
                    slots.Add(new Infrastructure.Data.Entities.Slot()
                    {
                        StartUtc = startTime,
                        EndUtc = startTime.AddMinutes(generatedSlotDurationInMinutes),
                        CreatedBy = command.ProviderId,
                        CreatedUtc = _dateTimeProvider.UtcNow(),
                        ProviderId = command.ProviderId,
                        CorrelationId = command.CorrelationId
                    });

                    startTime = startTime.AddMinutes(generatedSlotDurationInMinutes);
                }
            }
            else
            {
                while (startTime < endTime)
                {
                    // ensure a slot does not already exist prior to creating a new one
                    var slotExists = existingSlots.Any(p => p.StartUtc == startTime);
                    if (slotExists)
                    {
                        // log something?
                        startTime = startTime.AddMinutes(generatedSlotDurationInMinutes);
                        continue;
                    }

                    slots.Add(new Infrastructure.Data.Entities.Slot()
                    {
                        StartUtc = startTime,
                        EndUtc = startTime.AddMinutes(generatedSlotDurationInMinutes),
                        CreatedBy = command.ProviderId,
                        CreatedUtc = _dateTimeProvider.UtcNow(),
                        ProviderId = command.ProviderId,
                        CorrelationId = command.CorrelationId
                    });

                    startTime = startTime.AddMinutes(generatedSlotDurationInMinutes);
                }
            }

            return slots;
        }
    }
}

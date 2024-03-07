using Henry.Scheduling.Api.Common;

using System.Collections.Generic;
using System.Linq;

namespace Henry.Scheduling.Api.Application.Slot.Commands
{
    public interface IGenerateSlots
    {
        public List<Infrastructure.Data.Entities.Slot> GenerateSlots(
            int generatedSlotDurationInMinutes,
            CreateSlotsForProvider.Command command,
            List<Infrastructure.Data.Entities.Slot>? existingSlots);
    }

    public class SlotGenerator : IGenerateSlots
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SlotGenerator(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public List<Infrastructure.Data.Entities.Slot> GenerateSlots(
            int generatedSlotDurationInMinutes,
            CreateSlotsForProvider.Command command,
            List<Infrastructure.Data.Entities.Slot>? existingSlots)
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

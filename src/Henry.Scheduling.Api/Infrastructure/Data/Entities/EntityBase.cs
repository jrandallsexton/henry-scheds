using System;
using System.ComponentModel.DataAnnotations;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
{
    public abstract class EntityBase : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedUtc { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid CausationId { get; set; } = Guid.Empty;

        public Guid CorrelationId { get; set; } = Guid.Empty;

        public DateTime LastModified => ModifiedUtc ?? CreatedUtc;
    }
}
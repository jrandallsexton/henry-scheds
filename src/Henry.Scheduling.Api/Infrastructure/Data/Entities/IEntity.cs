using System;

namespace Henry.Scheduling.Api.Infrastructure.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedUtc { get; set; }
        DateTime? ModifiedUtc { get; set; }
        Guid CreatedBy { get; set; }
        Guid? ModifiedBy { get; set; }
    }
}

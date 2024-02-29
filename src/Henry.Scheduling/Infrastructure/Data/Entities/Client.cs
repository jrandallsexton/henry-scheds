using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Henry.Scheduling.Infrastructure.Data.Entities
{
    public class Client : EntityBase
    {
        public string Name { get; set; }

        public class EntityConfiguration : IEntityTypeConfiguration<Client>
        {
            public void Configure(EntityTypeBuilder<Client> builder)
            {
                builder.ToTable("Client");
                builder.HasKey(t => t.Id);
            }
        }
    }
}

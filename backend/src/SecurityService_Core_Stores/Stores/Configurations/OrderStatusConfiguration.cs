using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatusDB>
    {
        public void Configure(EntityTypeBuilder<OrderStatusDB> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("order_statuses", "public");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.OrderStatusName)
                .HasColumnName("order_status_name");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class UserStatusConfiguration : IEntityTypeConfiguration<UserStatusDB>
    {
        public void Configure(EntityTypeBuilder<UserStatusDB> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("user_statuses", "public");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.UserStatusName)
                .HasColumnName("user_status_name");
        }
    }
}

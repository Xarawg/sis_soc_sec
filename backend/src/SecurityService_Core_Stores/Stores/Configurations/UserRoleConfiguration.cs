using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;
using SecurityService_Core.Models.DTO;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleDB>
    {
        public void Configure(EntityTypeBuilder<UserRoleDB> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("user_roles", "public");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.UserRoleName)
                .HasColumnName("user_role_name");
        }
    }
}

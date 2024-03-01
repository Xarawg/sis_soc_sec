using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class UserHashConfiguration : IEntityTypeConfiguration<UserHashDB>
    {
        public void Configure(EntityTypeBuilder<UserHashDB> builder)
        {
            builder.HasKey(t => t.IdUser);

            builder.ToTable("user_hashes", "public");

            builder.Property(e => e.IdUser)
                .HasColumnName("id");

            builder.Property(e => e.UserName)
                .HasColumnName("login");

            builder.HasIndex(e => e.UserName)
                .IsUnique();

            builder.Property(e => e.Hash)
                .HasColumnName("hash");

            builder.HasIndex(e => e.Hash)
              .IsUnique();

            builder.Property(e => e.Salt)
                .HasColumnName("salt");

            builder.Property(e => e.Status)
                .HasColumnName("status");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class UserHashConfiguration : IEntityTypeConfiguration<UserHash>
    {
        public void Configure(EntityTypeBuilder<UserHash> builder)
        {
            builder.HasKey(t => t.IdUser);

            builder.ToTable("user_hashes", "public");

            builder.Property(e => e.IdUser)
                .HasColumnName("id");

            builder.Property(e => e.UserName)
                .HasColumnName("login");

            builder.Property(e => e.Hash)
                .HasColumnName("hash");

            builder.Property(e => e.Salt)
                .HasColumnName("salt");
            
            builder.Property(e => e.Status)
                .HasColumnName("status");
        }
    }
}

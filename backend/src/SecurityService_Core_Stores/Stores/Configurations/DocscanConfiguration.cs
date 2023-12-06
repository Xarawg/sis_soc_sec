using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class DocscanConfiguration : IEntityTypeConfiguration<Docscan>
    {
        public void Configure(EntityTypeBuilder<Docscan> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("docscans", "public");

            builder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");

            builder.Property(e => e.FileName)
                .HasColumnName("file_name");

            builder.Property(e => e.FileExt)
                .HasColumnName("file_ext");

            builder.Property(e => e.FileBody)
                .HasColumnName("file_body");

            builder.Property(e => e.ChangeDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("changedate");
            builder.Property(e => e.ChangeUser)
                .HasMaxLength(128)
                .HasColumnName("changeuser");
            builder.Property(e => e.CreateDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdate");
            builder.Property(e => e.CreateUser)
                .HasMaxLength(128)
                .HasColumnName("createuser");
        }
    }
}

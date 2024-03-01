using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderDB>
    {
        public void Configure(EntityTypeBuilder<OrderDB> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("orders", "public");

            builder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");

            builder.Property(e => e.Date)
                .HasColumnName("date");

            builder.Property(e => e.State)
                .HasColumnName("state");

            builder.Property(e => e.Status)
                .HasColumnName("status");

            builder.Property(e => e.SNILS)
                .HasColumnName("snils");

            builder.Property(e => e.FIO)
                .HasColumnName("fio");

            builder.Property(e => e.ContactData)
                .HasColumnName("contact_data");

            builder.Property(e => e.Type)
                .HasColumnName("type");

            builder.Property(e => e.Body)
                .HasColumnName("body");

            builder.Property(e => e.SupportMeasures)
                .HasColumnName("support_measures");

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

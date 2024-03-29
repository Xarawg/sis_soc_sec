﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecurityService_Core.Models.DB;

namespace SecurityService_Core_Stores.Stores.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDB>
    {
        public void Configure(EntityTypeBuilder<UserDB> builder)
        {
            builder.HasKey(t => t.Id);

            builder.ToTable("users", "public");

            builder.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");

            builder.Property(e => e.UserName)
                .HasColumnName("user_name");

            builder.HasIndex(e => e.UserName)
                .IsUnique();

            builder.Property(e => e.Email)
                .HasColumnName("email");

            builder.Property(e => e.EmailConfirmed)
                .HasColumnName("email_confirmed");

            builder.Property(e => e.PasswordHash)
                .HasColumnName("password_hash");

            builder.Property(e => e.SecurityStamp)
                .HasColumnName("security_stamp");

            builder.Property(e => e.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp");

            builder.Property(e => e.PhoneNumber)
                .HasColumnName("phone_number");

            builder.Property(e => e.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed");

            builder.Property(e => e.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled");

            builder.Property(e => e.AccessFailedAttemptDate)
                .HasColumnName("lockout_end");

            builder.Property(e => e.LockoutEnabled)
                .HasColumnName("lockout_enabled");

            builder.Property(e => e.AccessFailedCount)
                .HasColumnName("access_failed_count");

            builder.Property(e => e.UserRole)
                .HasColumnName("user_role");

            builder.Property(e => e.FIO)
                .HasColumnName("fio");

            builder.Property(e => e.Organization)
                .HasColumnName("organization");

            builder.Property(e => e.INN)
                .HasColumnName("inn");

            builder.Property(e => e.Address)
                .HasColumnName("address");

            builder.Property(e => e.State)
                .HasColumnName("state");

            builder.HasIndex(e => e.State);

            builder.Property(e => e.Status)
                .HasColumnName("status");

            builder.Property(e => e.IsTemporaryAccess)
                .HasColumnName("is_temporary_access");

            builder.Property(e => e.TemporaryAccessExpirationTime)
                .HasColumnName("temporary_access_expiration_time");

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

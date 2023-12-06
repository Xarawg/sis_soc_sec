﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SecurityService_Core_Stores;

#nullable disable

namespace SecurityService_Core_Stores.Migrations
{
    [DbContext(typeof(CustomerContext))]
    [Migration("20231205230253_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SecurityService_Core.Models.DB.Docscan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime?>("ChangeDate")
                        .HasColumnType("timestamp(3) without time zone")
                        .HasColumnName("changedate");

                    b.Property<string>("ChangeUser")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("changeuser");

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp(3) without time zone")
                        .HasColumnName("createdate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("createuser");

                    b.Property<byte[]>("FileBody")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("file_body");

                    b.Property<string>("FileExt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_ext");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.HasKey("Id");

                    b.ToTable("docscans", "public");
                });

            modelBuilder.Entity("SecurityService_Core.Models.DB.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Body")
                        .HasColumnType("text")
                        .HasColumnName("body");

                    b.Property<DateTime?>("ChangeDate")
                        .HasColumnType("timestamp(3) without time zone")
                        .HasColumnName("changedate");

                    b.Property<string>("ChangeUser")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("changeuser");

                    b.Property<string>("ContactData")
                        .HasColumnType("text")
                        .HasColumnName("contact_data");

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp(3) without time zone")
                        .HasColumnName("createdate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("createuser");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<string>("FIO")
                        .HasColumnType("text")
                        .HasColumnName("fio");

                    b.Property<string>("SNILS")
                        .HasColumnType("text")
                        .HasColumnName("snils");

                    b.Property<string>("State")
                        .HasColumnType("text")
                        .HasColumnName("state");

                    b.Property<string>("SupportMeasures")
                        .HasColumnType("text")
                        .HasColumnName("support_measures");

                    b.Property<string>("Type")
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("orders", "public");
                });

            modelBuilder.Entity("SecurityService_Core.Models.DB.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<DateTime?>("ChangeDate")
                        .HasColumnType("timestamp(3) without time zone")
                        .HasColumnName("changedate");

                    b.Property<string>("ChangeUser")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("changeuser");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp(3) without time zone")
                        .HasColumnName("createdate")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("createuser");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<string>("FIO")
                        .HasColumnType("text")
                        .HasColumnName("fio");

                    b.Property<string>("INN")
                        .HasColumnType("text")
                        .HasColumnName("inn");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTime?>("LockoutEnd")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("Organization")
                        .HasColumnType("text")
                        .HasColumnName("organization");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<int?>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.Property<int?>("UserRole")
                        .HasColumnType("integer")
                        .HasColumnName("user_role");

                    b.HasKey("Id");

                    b.HasIndex("Status");

                    b.ToTable("users", "public");
                });
#pragma warning restore 612, 618
        }
    }
}

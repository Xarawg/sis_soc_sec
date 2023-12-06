using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecurityService_Core_Stores.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.Sql(
            @"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");

            migrationBuilder.CreateTable(
                name: "docscans",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_ext = table.Column<string>(type: "text", nullable: false),
                    file_body = table.Column<byte[]>(type: "bytea", nullable: false),
                    createuser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    changeuser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    changedate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_docscans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    snils = table.Column<string>(type: "text", nullable: true),
                    fio = table.Column<string>(type: "text", nullable: true),
                    contact_data = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    body = table.Column<string>(type: "text", nullable: true),
                    support_measures = table.Column<string>(type: "text", nullable: true),
                    createuser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    changeuser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    changedate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false),
                    user_role = table.Column<int>(type: "integer", nullable: true),
                    fio = table.Column<string>(type: "text", nullable: true),
                    organization = table.Column<string>(type: "text", nullable: true),
                    inn = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    createuser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    changeuser = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    createdate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    changedate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_status",
                schema: "public",
                table: "users",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "docscans",
                schema: "public");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}

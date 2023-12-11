using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SecurityService_Core_Stores.Migrations
{
    /// <inheritdoc />
    public partial class IniticalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "docscans",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IdOrder = table.Column<Guid>(type: "uuid", nullable: false),
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
                name: "order_statuses",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_status_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    state = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: true),
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
                name: "user_hashes",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    login = table.Column<string>(type: "text", nullable: true),
                    hash = table.Column<byte[]>(type: "bytea", nullable: true),
                    salt = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_hashes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_statuses",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_status_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_statuses", x => x.id);
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
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: true),
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
                    is_temporary_access = table.Column<bool>(type: "boolean", nullable: true),
                    temporary_access_expiration_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                name: "IX_user_hashes_hash",
                schema: "public",
                table: "user_hashes",
                column: "hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_hashes_login",
                schema: "public",
                table: "user_hashes",
                column: "login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_status",
                schema: "public",
                table: "users",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_name",
                schema: "public",
                table: "users",
                column: "user_name",
                unique: true);

            migrationBuilder.Sql(
            @"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");

            migrationBuilder.Sql(
                @"INSERT INTO public.user_hashes (id,login,hash,salt,status) VALUES
                ('fe475961-1fbb-490c-b5c9-dfcc69064265','SuperAdmin',decode('9EA9FAEFD707D6D88CDC6828EB3592DACBAD1F50E530F5425AB19AEE30E0AE30','hex'),
                'sQclp1f0M+SPBGHUFINy9bMDzQsdOGMvD4WotzyXSco=',1);"
            );

            migrationBuilder.Sql(
                @"INSERT INTO public.users (id,user_name,email,email_confirmed,password_hash,security_stamp,concurrency_stamp,phone_number,
                phone_number_confirmed,two_factor_enabled,lockout_end,lockout_enabled,access_failed_count,user_role,fio,organization,inn,
                address,status,is_temporary_access,temporary_access_expiration_time,createuser,changeuser,createdate,changedate) VALUES
                ('fe475961-1fbb-490c-b5c9-dfcc69064265','SuperAdmin','super-admin@test.com',true,
                decode('9EA9FAEFD707D6D88CDC6828EB3592DACBAD1F50E530F5425AB19AEE30E0AE30','hex'),NULL,NULL,'12345678901',true,true,NULL,false,
                0,3,'Иванов Иван Иванович','ИП Иванов И.И.','1234567890','г. Иваново, ул. Ивана Грозного, д. 1, кв. 1',1,NULL,NULL,'SuperAdmin',
                NULL,'2023-12-11 08:07:37.352',NULL);"
            );

            migrationBuilder.Sql(
                @"INSERT INTO public.user_roles (id, user_role_name) VALUES
               ('0', 'Отсутствует'),
               ('1', 'Оператор'),
               ('2', 'Администратор'),
               ('3', 'Супер администратор');"
            );

            migrationBuilder.Sql(
                @"INSERT INTO public.user_statuses (id, user_status_name) VALUES
               ('0', 'Вновь заведенный пользователь'),
               ('1', 'Зарегистрированный пользователь'),
               ('-1', 'Отклоненный'),
               ('-2', 'Заблокированный');"
            );

            migrationBuilder.Sql(
                @"INSERT INTO public.order_statuses (id, order_status_name) VALUES
               ('0', 'Вновь заведенная заявка'),
               ('1', 'Отправленная в обработку'),
               ('2', 'Успешно обработанная'),
               ('-1', 'Отклоненная заявка'),
               ('-2', 'Отмененная пользователем'),
               ('-3', 'Ошибка в заявке');"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "docscans",
                schema: "public");

            migrationBuilder.DropTable(
                name: "order_statuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_hashes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_statuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodegem.WebApi.Migrations.Sqlite.Nodegem
{
    public partial class DefaultLocalUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d04088b0-6155-42d6-aaff-729ac9ac7a40"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b65405f0-c821-4beb-a97e-a9eced10828f"), 0, null, "5a600df4-58e7-47bd-add9-f625c1f1ef81", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEME5FcADy4Uorw0oYXRmO0Px2SxtHYq6gzDIT3L+aG66a1/4A56sqHIVRYzDBTdRGA==", null, true, "03c4a1db830a4430a667d8ac2782688b", false, "AdminUser" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("28f6458d-9f64-4e8c-aa17-10d8387752ba"), 0, null, "7d0b4834-5694-4b22-96b9-76f2c0108788", "[]", new DateTime(2019, 12, 20, 16, 17, 43, 803, DateTimeKind.Utc).AddTicks(6935), "nodegemdefault@nodegem.io", true, "Nodegem", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Default", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "NODEGEMDEFAULT@NODEGEM.IO", "NODEGEM_DEFAULT", "AQAAAAEAACcQAAAAEN1gbk3gjHpnEEvKXUzobP9+hvcslV/AYHF8EVsJ0cLwyxiiTEReAFUGSkMwu35z4A==", null, false, "c86c7701-dba7-4b4a-b730-7bd919922791", false, "Nodegem_Default" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("28f6458d-9f64-4e8c-aa17-10d8387752ba"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b65405f0-c821-4beb-a97e-a9eced10828f"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d04088b0-6155-42d6-aaff-729ac9ac7a40"), 0, null, "2e099945-7f2a-49c1-ac92-0d6656d56943", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEFenspfVrtwtmyp9DPXZzuVddP41gcdJmOzDtLQsJ53YJMwU35fgz4PHjgF95Z6AJA==", null, true, "7e22ecde5243476e91a30b4f80378e54", false, "AdminUser" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodester.WebApi.Migrations
{
    public partial class TokenNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e028f0cf-1b93-4695-855a-8f322b2b0b59"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("8cd28a3b-c611-4cd5-9891-48f7bac68c99"), 0, "3e03d671-1b03-46a8-9fcc-f579d50589bb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEI+e0G3TED0sSh2j7TtptJqfvWPLZWM5gTyixCjBNEWD1zErx7B1W+HQobLPlZ9AjQ==", null, true, "231bd0fa51c34745be7a8fb596b27eb7", false, "AdminUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8cd28a3b-c611-4cd5-9891-48f7bac68c99"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e028f0cf-1b93-4695-855a-8f322b2b0b59"), 0, "d7ad1189-ad05-491e-aa3a-ff766ccb92b4", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEBBQKucHnl7LCQvgcB/Sd4wXypwVgqAJGY6Lrc1ikKGHdQVEydgH9Zc+8LfSzNNxWg==", null, true, "02bafb2465a64c26a55889622aee9049", false, "AdminUser" });
        }
    }
}

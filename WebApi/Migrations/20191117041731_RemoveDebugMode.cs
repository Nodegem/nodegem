using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodester.WebApi.Migrations
{
    public partial class RemoveDebugMode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c962c8ae-aaac-4a0b-aea4-1f13f06df12a"));

            migrationBuilder.DropColumn(
                name: "IsDebugModeEnabled",
                table: "Macros");

            migrationBuilder.DropColumn(
                name: "IsDebugModeEnabled",
                table: "Graphs");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("00cbed10-b610-459c-bc1d-4e5fb487964c"), 0, null, "84d9f4bc-e3ff-4a7a-a2c8-aae94cdfbe7d", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEOMOQTpOM2RnDuFwLYXmOx11MUxYxQPQTC4/QhQGE+v4YR9o1Mf8jVIEfDGHV/6ETQ==", null, true, "35a409194991438ab2277141da3a53cf", false, "AdminUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00cbed10-b610-459c-bc1d-4e5fb487964c"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDebugModeEnabled",
                table: "Macros",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDebugModeEnabled",
                table: "Graphs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("c962c8ae-aaac-4a0b-aea4-1f13f06df12a"), 0, null, "f9af2cc0-ee6b-4115-9526-ce931a34f2a7", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEEEN3CwmfH0/+HE5jYg9F8Leq7ti8k3/ilsyLZFKPYkVhFTxd55ax49+cIll0UNLsg==", null, true, "5d4472a5b2204f3a970a61c6a2c8f6c8", false, "AdminUser" });
        }
    }
}

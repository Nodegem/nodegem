using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodester.WebApi.Migrations
{
    public partial class AddingDebugFlagToGraphs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("197f0b1c-ae56-4f95-b228-e333be5b16a5"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDebugModeEnabled",
                table: "Macros",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDebugModeEnabled",
                table: "Graphs",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("1659ef01-3310-430d-93ff-5ff980d167a5"), 0, "62b3bca5-5354-4534-b17b-f8eafb9ffd08", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEJ1UzPn1SY7Zote+/eNmTRH+woMNjvZZCpb+MghpYzmmTCoSBj3E+RG2oP2guCrEoQ==", null, true, "3940e39de7cb470082d950b597896376", false, "AdminUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1659ef01-3310-430d-93ff-5ff980d167a5"));

            migrationBuilder.DropColumn(
                name: "IsDebugModeEnabled",
                table: "Macros");

            migrationBuilder.DropColumn(
                name: "IsDebugModeEnabled",
                table: "Graphs");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("197f0b1c-ae56-4f95-b228-e333be5b16a5"), 0, "2dcdabeb-7530-4bbe-a75c-84db65dd3a80", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEM7qrAKEqilRw8NiBCWj2Eju2VDJm/DqbkTgqDsBnCHvUg7j78td+xcFPHi92q7G7A==", null, true, "27cfa45fc25a41bba2106cde1863551a", false, "AdminUser" });
        }
    }
}

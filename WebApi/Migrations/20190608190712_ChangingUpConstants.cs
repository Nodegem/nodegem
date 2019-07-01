using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodester.WebApi.Migrations
{
    public partial class ChangingUpConstants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d312c9bf-77ca-4a80-a8f7-d8da3517db10"));

            migrationBuilder.DropColumn(
                name: "Constants",
                table: "Macros");

            migrationBuilder.DropColumn(
                name: "EnvironmentVariables",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Constants",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("197f0b1c-ae56-4f95-b228-e333be5b16a5"), 0, "2dcdabeb-7530-4bbe-a75c-84db65dd3a80", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEM7qrAKEqilRw8NiBCWj2Eju2VDJm/DqbkTgqDsBnCHvUg7j78td+xcFPHi92q7G7A==", null, true, "27cfa45fc25a41bba2106cde1863551a", false, "AdminUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("197f0b1c-ae56-4f95-b228-e333be5b16a5"));

            migrationBuilder.DropColumn(
                name: "Constants",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Constants",
                table: "Macros",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnvironmentVariables",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d312c9bf-77ca-4a80-a8f7-d8da3517db10"), 0, "9abdffff-fe8a-4caa-9f50-02e7479e86e6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEFou9VSRRlAXG6bl6IEA0od7bBDvx3IwtfaPrFckupLqdUgYkODLHbtu0ewOzbwhiw==", null, true, "a426ef8d90674feea1a7a54623df7f6b", false, "AdminUser" });
        }
    }
}

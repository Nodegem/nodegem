using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodester.WebApi.Migrations
{
    public partial class UpdatingAdminUserSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("96f4f5ea-6a98-4970-bcb4-bf343ab22627"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed",
                    "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName",
                    "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName",
                    "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled",
                    "UserName"
                },
                values: new object[]
                {
                    new Guid("48214840-f9e8-4085-8144-b9277a42f6eb"), 0, "32a1da35-5a48-4b28-be5b-c332ca6294c2",
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null,
                    true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null,
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM",
                    "ADMINUSER", "AQAAAAEAACcQAAAAEMLiU8+yyVw9DbNEX9QiLS9h/Yy3t6BLTYIsoj1PrQIGVZmKZjuuksU5/ekxC7y4lg==",
                    null, true, "ea5e3277ef0d4e92ab45598536885410", false, "AdminUser"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("48214840-f9e8-4085-8144-b9277a42f6eb"));

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed",
                    "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName",
                    "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName",
                    "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled",
                    "UserName"
                },
                values: new object[]
                {
                    new Guid("96f4f5ea-6a98-4970-bcb4-bf343ab22627"), 0, "c5f230cf-71c1-4a21-ba48-e0ddcc17bac0",
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null,
                    true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null,
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null,
                    "AQAAAAEAACcQAAAAEExT1zoHVTV28k3gCqhf3Jwiy27Q6PJPIqcem4hSA4MseC2AOhc9qSpiKn5a8BgxNA==", null, true,
                    null, false, "AdminUser"
                });
        }
    }
}
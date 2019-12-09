using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodegem.WebApi.Migrations
{
    public partial class RemovingBSPropertyThatIForSomeReasonAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9722d99c-b3f1-4df3-a5ed-7d30f7b110d1"));

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("006ddf56-3d74-4eeb-b1e5-a068b98a0ef1"), 0, null, "b0705d3c-0e82-49d6-93ad-a3b44206c069", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAENFK6q6SgCIDLP4pME2me8EV1GZ0HQZDRTa/5vSM+uydBN8paRRiFRx2QltJyk09iw==", null, true, "c140e84a27014edaa0e7547667e0f3d0", false, "AdminUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("006ddf56-3d74-4eeb-b1e5-a068b98a0ef1"));

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("9722d99c-b3f1-4df3-a5ed-7d30f7b110d1"), 0, null, "73b5d1af-b534-4105-9204-89f24ea5f8af", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEODD9sO3fSHgscrYd9qAoZbSC/MEf0gUzZmsWIf4ewrveu/XjwkQK8J8LHgs7Zefzw==", null, true, "7c3191be632f4a51a4cb75c3266769d8", false, "AdminUser" });
        }
    }
}

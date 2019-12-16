using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodegem.WebApi.Migrations
{
    public partial class GraphMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7baf3ea1-607e-46c7-b707-5437ccdea2fb"));

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Macros",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Graphs",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("9722d99c-b3f1-4df3-a5ed-7d30f7b110d1"), 0, null, "73b5d1af-b534-4105-9204-89f24ea5f8af", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEODD9sO3fSHgscrYd9qAoZbSC/MEf0gUzZmsWIf4ewrveu/XjwkQK8J8LHgs7Zefzw==", null, true, "7c3191be632f4a51a4cb75c3266769d8", false, "AdminUser" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9722d99c-b3f1-4df3-a5ed-7d30f7b110d1"));

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Macros");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Graphs");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarUrl", "ConcurrencyStamp", "Constants", "CreatedOn", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("7baf3ea1-607e-46c7-b707-5437ccdea2fb"), 0, null, "3b4fef80-dfd2-4a64-b973-1263e796fd69", "[]", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEHba2+Fq/Kzwuk0+dmN2XiD0oKPqeqESdTsYOYdO54ZsUF+EcprfkrMOyT3LWQAIAg==", null, true, "a9d796b92e6f4c46a5ed9e317cbe8e5d", false, "AdminUser" });
        }
    }
}

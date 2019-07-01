using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nodester.WebApi.Migrations
{
    public partial class ExpiresUtcToRefreshTokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("48214840-f9e8-4085-8144-b9277a42f6eb"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresUtc",
                table: "RefreshTokens",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                    name: "Id",
                    table: "AspNetUserClaims",
                    nullable: false,
                    oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                    name: "Id",
                    table: "AspNetRoleClaims",
                    nullable: false,
                    oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

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
                    new Guid("a04ff2cb-89fc-4ea1-a9d4-c90eb30a53cb"), 0, "5249e7e0-e569-4483-bbc7-818a974468b9",
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null,
                    true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null,
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM",
                    "ADMINUSER", "AQAAAAEAACcQAAAAEMBarHGkYlNMMfe74LG2L/uGD2e5SShGuzDzp8HJOupzE/6MFYmS6SxBgf70h8Bh1g==",
                    null, true, "38cf62fa03674b6981a00b3d5904ba58", false, "AdminUser"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a04ff2cb-89fc-4ea1-a9d4-c90eb30a53cb"));

            migrationBuilder.DropColumn(
                name: "ExpiresUtc",
                table: "RefreshTokens");

            migrationBuilder.AlterColumn<int>(
                    name: "Id",
                    table: "AspNetUserClaims",
                    nullable: false,
                    oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AlterColumn<int>(
                    name: "Id",
                    table: "AspNetRoleClaims",
                    nullable: false,
                    oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

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
    }
}
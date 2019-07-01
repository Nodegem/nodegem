using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nodester.WebApi.Migrations
{
    public partial class ActualTokenNameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RefreshTokens_Token",
                table: "RefreshTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8cd28a3b-c611-4cd5-9891-48f7bac68c99"));

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "AccessTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessTokens",
                table: "AccessTokens",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccessTokens_Token",
                table: "AccessTokens",
                column: "Token");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccessTokens_UserId",
                table: "AccessTokens",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d312c9bf-77ca-4a80-a8f7-d8da3517db10"), 0, "9abdffff-fe8a-4caa-9f50-02e7479e86e6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEFou9VSRRlAXG6bl6IEA0od7bBDvx3IwtfaPrFckupLqdUgYkODLHbtu0ewOzbwhiw==", null, true, "a426ef8d90674feea1a7a54623df7f6b", false, "AdminUser" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccessTokens_AspNetUsers_UserId",
                table: "AccessTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessTokens_AspNetUsers_UserId",
                table: "AccessTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessTokens",
                table: "AccessTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccessTokens_Token",
                table: "AccessTokens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccessTokens_UserId",
                table: "AccessTokens");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d312c9bf-77ca-4a80-a8f7-d8da3517db10"));

            migrationBuilder.RenameTable(
                name: "AccessTokens",
                newName: "RefreshTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedOn", "Email", "EmailConfirmed", "EnvironmentVariables", "FirstName", "IsActive", "IsLocked", "LastLoggedIn", "LastName", "LastUpdated", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("8cd28a3b-c611-4cd5-9891-48f7bac68c99"), 0, "3e03d671-1b03-46a8-9fcc-f579d50589bb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", true, null, null, true, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "ADMIN@ADMIN.COM", "ADMINUSER", "AQAAAAEAACcQAAAAEI+e0G3TED0sSh2j7TtptJqfvWPLZWM5gTyixCjBNEWD1zErx7B1W+HQobLPlZ9AjQ==", null, true, "231bd0fa51c34745be7a8fb596b27eb7", false, "AdminUser" });

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

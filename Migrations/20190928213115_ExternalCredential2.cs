using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class ExternalCredential2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCredential_AspNetUsers_ApplicationUserId",
                table: "ExternalCredential");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalCredential",
                table: "ExternalCredential");

            migrationBuilder.RenameTable(
                name: "ExternalCredential",
                newName: "ExternalCredentials");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalCredential_ApplicationUserId",
                table: "ExternalCredentials",
                newName: "IX_ExternalCredentials_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalCredentials",
                table: "ExternalCredentials",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "1040f4cd-80aa-4c04-8af4-cddb3b3ec498");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7b51c078-1d16-450c-8eb8-99b1619d26a0", "AQAAAAEAACcQAAAAEDERTAbDI9QJoe493YJIRHrpdnFyE7ANnEAYRkT6ma76HIypyqEHN/zYvKrbFbWnKQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCredentials_AspNetUsers_ApplicationUserId",
                table: "ExternalCredentials",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalCredentials_AspNetUsers_ApplicationUserId",
                table: "ExternalCredentials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExternalCredentials",
                table: "ExternalCredentials");

            migrationBuilder.RenameTable(
                name: "ExternalCredentials",
                newName: "ExternalCredential");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalCredentials_ApplicationUserId",
                table: "ExternalCredential",
                newName: "IX_ExternalCredential_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExternalCredential",
                table: "ExternalCredential",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "d1522a92-7fcd-4351-a8b8-8b07511ff839");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c3b753f6-0fc2-4f73-9f02-fca0fc3c7c4b", "AQAAAAEAACcQAAAAEMb7h1sfJTQ5vsa+CgI8I4ndAo0dTOa3XF22OxM3LHLNo3sNz/G0zEz5/cWLaRKipQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalCredential_AspNetUsers_ApplicationUserId",
                table: "ExternalCredential",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

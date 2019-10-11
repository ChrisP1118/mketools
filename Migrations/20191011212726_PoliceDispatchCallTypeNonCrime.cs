using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class PoliceDispatchCallTypeNonCrime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOtherCrime",
                table: "PoliceDispatchCallTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "1b532dc1-b3ff-4bb9-abaa-6c44b3a9131e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "852e42e8-fe76-451c-9e4c-dfe182d012f1", "AQAAAAEAACcQAAAAENo5Zp86vk2L01hwMB0g5Kq5O+MX5Npeq7q24T6UmvMjlkjmPH92db1tsAmUQrBCpA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOtherCrime",
                table: "PoliceDispatchCallTypes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "6f24baa3-26eb-4e21-bd6c-a1eaf8a56968");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "05f3905a-957f-4270-aaf3-a11e7c7c3449", "AQAAAAEAACcQAAAAEH5rga3bMddB0rwoQD7uVhjLS1O8DE/pP9abHT72kfOWb8bFE2+QpBfJQGf6H890sg==" });
        }
    }
}

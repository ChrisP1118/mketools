using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class PropertySourceIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "89e8d56a-2e94-4048-8ebd-b9d9d7b98094");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "25e92ba4-082a-497a-aeba-4004b1f3d90f", "AQAAAAEAACcQAAAAEE42PQ2QuEw7oyYhpbNoZDVJy/uCLOP9OI5NbD0832PegIkLKgVe7solx0JRQZnIGQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Source",
                table: "Properties",
                column: "Source");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_Source",
                table: "Properties");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "a09bf2be-7b0b-484a-91b0-375794e79457");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f1ee38ab-c196-4429-a2cd-9d575e6b560c", "AQAAAAEAACcQAAAAEGaHlTBP4+iH+4glqZa7jZAqCtRmjbqomtl53Lx5xMklxWgMr0tFiRFzp5qGYA8OoA==" });
        }
    }
}

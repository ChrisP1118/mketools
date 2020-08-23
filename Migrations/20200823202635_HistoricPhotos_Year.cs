using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class HistoricPhotos_Year : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "HistoricPhotos",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "f367bc10-39b7-4039-aede-808be849108c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "69bf8f79-afb1-484f-878d-fdfb7d26c19b", "AQAAAAEAACcQAAAAEJ6ZHuKZgyd07ccZnYItkHG4Ix6PENDBjfQqyvjTo3bH0dvShvtzZVlxE4sDK2rbCw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "HistoricPhotos");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "1280b310-51d7-4b50-98c3-57239f2af839");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6fd5f273-c68a-4efb-95e7-3202bcb48635", "AQAAAAEAACcQAAAAEMaBjUHtNctxRzokmq+8iSn54/GkHyQNv8ZEtnseggldDibsjoNnSSZdtsCw2nrXPw==" });
        }
    }
}

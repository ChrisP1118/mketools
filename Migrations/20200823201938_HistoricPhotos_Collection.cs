using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class HistoricPhotos_Collection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Collection",
                table: "HistoricPhotos",
                maxLength: 12,
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collection",
                table: "HistoricPhotos");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "0e5ee386-1e9e-4c33-b5a8-0b4670bdf40a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8271bede-d698-4fca-955d-681cd07edd49", "AQAAAAEAACcQAAAAEO5rPYImDK0lATSnw9aiwNi52VQcpH3dKMlFAXuGICxu0ZInOmQRTN/OD1lVQxB9CQ==" });
        }
    }
}

using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class FireDispatchCalls2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FireDispatchCalls",
                columns: table => new
                {
                    CFS = table.Column<string>(maxLength: 12, nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(maxLength: 60, nullable: false),
                    City = table.Column<string>(maxLength: 50, nullable: true),
                    NatureOfCall = table.Column<string>(maxLength: 20, nullable: false),
                    Disposition = table.Column<string>(maxLength: 60, nullable: true),
                    Geometry = table.Column<IGeometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Accuracy = table.Column<int>(nullable: false),
                    Source = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireDispatchCalls", x => x.CFS);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "e36c25ee-2881-4e7f-a225-840ca1124474");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "be4f9f51-c1d0-42c4-9c55-158aec43010c", "AQAAAAEAACcQAAAAEJbT/XHVgjNb7lEaBfSs9HF7MPrmI6Tg2ksl0r0AY7BSvlnXcgbKfzigAn82lmSJAw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FireDispatchCalls");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "70706bcb-bb31-457f-98fa-0d53700d367d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0aed4972-fd32-4e04-a1d4-1222fad97d21", "AQAAAAEAACcQAAAAEKtPEfBeInP9jIcR/9+cTowOdVLeUVWb6nKM6OrgQQwaCwMkbz/sqVU/U3dh5gT7Ew==" });
        }
    }
}

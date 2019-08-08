using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class DispatchCall_Geodata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IPoint>(
                name: "Centroid",
                table: "DispatchCalls",
                nullable: true);

            migrationBuilder.AddColumn<IGeometry>(
                name: "Outline",
                table: "DispatchCalls",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "bf7a19a4-96db-47c5-861d-d113d7b23a8b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "aada7329-fe26-4e27-a5ed-97b217dea5f6", "AQAAAAEAACcQAAAAECueGf9ZZ+ArEIwppgwR4HyqEDIUszpEYDB1NAd1a0bB56r/YXj4Qhr376XbECS+/g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Centroid",
                table: "DispatchCalls");

            migrationBuilder.DropColumn(
                name: "Outline",
                table: "DispatchCalls");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "0cf1c807-6734-443d-afe0-7322a1e07202");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "afa46765-a248-4948-a816-e93d58970550", "AQAAAAEAACcQAAAAEEcE98CMjwAKK/oobIhe1/IPvjmDxm0ggUnaXrIk/rhagjrGWy0kuZVCS7ztEf+Uww==" });
        }
    }
}

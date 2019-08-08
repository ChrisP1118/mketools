using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class DispatchCall_Geodata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Centroid",
                table: "DispatchCalls");

            migrationBuilder.RenameColumn(
                name: "Outline",
                table: "DispatchCalls",
                newName: "Geometry");

            migrationBuilder.AddColumn<int>(
                name: "Accuracy",
                table: "DispatchCalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "DispatchCalls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "baad8cd9-f0a6-44f5-9b4e-570504b981c3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1970d04d-1eee-4e7c-a5b0-ccd4d6c2e23d", "AQAAAAEAACcQAAAAEDHquZ/gA6BUS5rCV9AApLDQcfl0ri2s/ZjCsiDDj8gkcO9Pj30a47SuAd0wvgbPGA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "DispatchCalls");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "DispatchCalls");

            migrationBuilder.RenameColumn(
                name: "Geometry",
                table: "DispatchCalls",
                newName: "Outline");

            migrationBuilder.AddColumn<IPoint>(
                name: "Centroid",
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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class Bounds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxLat",
                table: "DispatchCalls",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxLng",
                table: "DispatchCalls",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLat",
                table: "DispatchCalls",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLng",
                table: "DispatchCalls",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxLat",
                table: "Crimes",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxLng",
                table: "Crimes",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLat",
                table: "Crimes",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLng",
                table: "Crimes",
                type: "decimal(5, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "3a98abb7-4625-406b-bde0-fc70d8aa1487");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "79bf4325-37ff-4e5f-b781-de0b7dc4d185", "AQAAAAEAACcQAAAAEOTVF3I+piymslOZc9jmaFEoCBKCZxh3aIuzipb/zSdV0kJRlk+OVq17GkLrXuG30g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxLat",
                table: "DispatchCalls");

            migrationBuilder.DropColumn(
                name: "MaxLng",
                table: "DispatchCalls");

            migrationBuilder.DropColumn(
                name: "MinLat",
                table: "DispatchCalls");

            migrationBuilder.DropColumn(
                name: "MinLng",
                table: "DispatchCalls");

            migrationBuilder.DropColumn(
                name: "MaxLat",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "MaxLng",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "MinLat",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "MinLng",
                table: "Crimes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "9c0e9687-af42-4d9c-be48-c5a3413b113e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "31c8b4fa-8eef-413b-b98e-a3cd721ec381", "AQAAAAEAACcQAAAAEBi4e9qjn43m5keehXMQlTpU8LbMaHSRC/c64iZR0Ltu3wTZc8oOqjF+2iG5C80PqQ==" });
        }
    }
}

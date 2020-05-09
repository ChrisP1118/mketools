using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxLat",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "MaxLng",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "MinLat",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "MinLng",
                table: "Parcels");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "cbb524fb-29a5-42b6-bb22-2588b768b9b0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c4be5479-f0d3-4c94-bdf1-d9d7f2436a40", "AQAAAAEAACcQAAAAEGymhYLE87/Yr0yUjybXShOP9W71tusdM6c+4Ji4qX5Bu+HJOpQhE2z3fI5AiEv2jw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MaxLat",
                table: "Parcels",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxLng",
                table: "Parcels",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLat",
                table: "Parcels",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLng",
                table: "Parcels",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "e909918a-cd7e-430a-a755-ece85c836e1b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d5c41c9c-bb3f-4d48-852e-561181cb1963", "AQAAAAEAACcQAAAAEKSL1hEhkNfMHWL96Vl2GgIGahgLgmegiR/orxlRVQls/1Sz+1X8zDnRJy4D8aHSJg==" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeftNumberHigh",
                table: "Streets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeftNumberLow",
                table: "Streets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightNumberHigh",
                table: "Streets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RightNumberLow",
                table: "Streets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HouseNumber",
                table: "Parcels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HouseNumberHigh",
                table: "Parcels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "02ed99b0-536d-471d-8672-612e036e3f20");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b49f4a80-974c-4c7d-9bf5-e1dae1071ba3", "AQAAAAEAACcQAAAAEN1Iez9ivDuQILfrrkAsXITG7DbeWYR/NCw9JxET3sO+5XG+6gPBDLkXTvR0KxMvvw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftNumberHigh",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "LeftNumberLow",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "RightNumberHigh",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "RightNumberLow",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Parcels");

            migrationBuilder.DropColumn(
                name: "HouseNumberHigh",
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
    }
}

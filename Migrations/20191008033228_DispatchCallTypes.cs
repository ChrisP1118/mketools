using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class DispatchCallTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NatureOfCall",
                table: "FireDispatchCalls",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "FireDispatchCallTypes",
                columns: table => new
                {
                    NatureOfCall = table.Column<string>(maxLength: 40, nullable: false),
                    IsCritical = table.Column<bool>(nullable: false),
                    IsFire = table.Column<bool>(nullable: false),
                    IsMedical = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireDispatchCallTypes", x => x.NatureOfCall);
                });

            migrationBuilder.CreateTable(
                name: "PoliceDispatchCallTypes",
                columns: table => new
                {
                    NatureOfCall = table.Column<string>(maxLength: 20, nullable: false),
                    IsCritical = table.Column<bool>(nullable: false),
                    IsViolent = table.Column<bool>(nullable: false),
                    IsProperty = table.Column<bool>(nullable: false),
                    IsDrug = table.Column<bool>(nullable: false),
                    IsTraffic = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceDispatchCallTypes", x => x.NatureOfCall);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "2d099274-e775-43c3-87a5-0ac562c64c7a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b1a9e08f-2e32-4a98-8310-e65f1ad1be6a", "AQAAAAEAACcQAAAAEHYbw/ZDJqyPKPbaaGnnzl2q7BW/ANN+RoWe1pgUQXohBKCGU9ECEmdD5t+sxNA+rA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FireDispatchCallTypes");

            migrationBuilder.DropTable(
                name: "PoliceDispatchCallTypes");

            migrationBuilder.AlterColumn<string>(
                name: "NatureOfCall",
                table: "FireDispatchCalls",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "11d8b4ca-09b5-4794-a76d-138bac214ee2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9e6ef23d-c437-4083-9309-c0c23600eca6", "AQAAAAEAACcQAAAAELufMJGyHbs8PJyiaTjaKEpQILZfWbjII4wNv7nM1CiIVZEnGUkKFGD4oivzczY7Pg==" });
        }
    }
}

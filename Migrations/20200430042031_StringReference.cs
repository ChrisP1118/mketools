using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class StringReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StringReference");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "59589b40-bfd3-4db6-8cb6-d5c6442b850c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dcc9c42f-78fa-4753-9edf-f5baab2adb3a", "AQAAAAEAACcQAAAAEHkM3Tbv0Nd5gs62b3ZVTE4NAqWh/xtFjcUAfXiDtiaBpjTy+h522CfTLnCxGR8kag==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StringReference",
                columns: table => new
                {
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "7dfc2af2-df08-48e1-af40-d6187afab59f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6488f362-61e4-4218-97d4-beb3d1cbe643", "AQAAAAEAACcQAAAAEMP1yMgQRYqGP53J1VduKBIx+HJT1X8VVXPrNWHfdjy+yxyI6o92TyxYhJoVQfLczw==" });
        }
    }
}

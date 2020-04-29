using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class BoundsPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "Streets",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "Streets",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "Streets",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "Streets",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "PoliceDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "PoliceDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "PoliceDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "PoliceDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "FireDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "FireDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "FireDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "FireDispatchCalls",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "Crimes",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "Crimes",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "Crimes",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "Crimes",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "CommonParcels",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "CommonParcels",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "CommonParcels",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "CommonParcels",
                type: "decimal(13, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5, 10)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "3b653776-39db-41b4-baf5-a05e154535fe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f0fd19dc-b858-4a40-b84e-2357924483e2", "AQAAAAEAACcQAAAAEJ20llgKcuy+lVCGninGyRtrS9USX8Zo6YAF53tAKtLXgc2RPntdBadX7Kv5UDK8ww==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "Streets",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "Streets",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "Streets",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "Streets",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "PoliceDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "PoliceDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "PoliceDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "PoliceDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "FireDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "FireDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "FireDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "FireDispatchCalls",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "Crimes",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "Crimes",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "Crimes",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "Crimes",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLng",
                table: "CommonParcels",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinLat",
                table: "CommonParcels",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLng",
                table: "CommonParcels",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaxLat",
                table: "CommonParcels",
                type: "decimal(5, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(13, 10)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "713c0057-fd64-4f45-8ab7-b594c7d56e30");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5cd89bc1-2cb7-40c9-b0b4-96334d34c074", "AQAAAAEAACcQAAAAEF6KWJDlz5FNDchYcv07cikzV8COSY+g1UEviEvX0zvyzoRufdLxr/+JUcYhKyZlBw==" });
        }
    }
}

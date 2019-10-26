using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "PercentAir",
                table: "Parcels",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<float>(
                name: "Calculat_1",
                table: "Parcels",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "f2f1cdac-0b2c-4c9e-9d4a-323b91093eff");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a91c71e3-5e20-4cd8-b7e6-0ada73835ca3", "AQAAAAEAACcQAAAAEPel9MEt3DaAcAKioAAbNNH6U+Z5tcn1geGuxpGitAEkVSSK+tGzW8b6Utmvua8olw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PercentAir",
                table: "Parcels",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<int>(
                name: "Calculat_1",
                table: "Parcels",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "19aa1538-bc60-44ca-bc39-1922d4778d0f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "33ddb975-493a-4b58-ae7a-c52c31ce1f2a", "AQAAAAEAACcQAAAAEADtoBi7KCmV2tLC6kGm7vmXzH8KP8Lb01WGcGPhABEJFa2gRFlR/qa6EMyY8sJ5og==" });
        }
    }
}

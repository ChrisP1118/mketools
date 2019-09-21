using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class FireDispatchCalls3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apt",
                table: "FireDispatchCalls",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "1e4ee0c7-99d8-4601-aae8-2c8123ec5d9b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5adddb64-ed0d-4baa-b4ce-3134ce427f4d", "AQAAAAEAACcQAAAAEJ3/06Ny5hSHc415s1N+BEdW2eqABawuKqhKvEA7D10pu/dfGol5Uy01AW+/jpcl4g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apt",
                table: "FireDispatchCalls");

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
    }
}

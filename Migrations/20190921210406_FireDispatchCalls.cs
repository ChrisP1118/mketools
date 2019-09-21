using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class FireDispatchCalls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeOfCrime",
                table: "Crimes",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "70706bcb-bb31-457f-98fa-0d53700d367d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0aed4972-fd32-4e04-a1d4-1222fad97d21", "AQAAAAEAACcQAAAAEKtPEfBeInP9jIcR/9+cTowOdVLeUVWb6nKM6OrgQQwaCwMkbz/sqVU/U3dh5gT7Ew==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfCrime",
                table: "Crimes");

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class RemoveCrimeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfCrime",
                table: "Crimes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "2824f47c-0a1a-42da-ace2-7b26e81a1c12");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "37409e64-d6fb-48f0-9657-e391fa3598f9", "AQAAAAEAACcQAAAAEGawm3QgB9B0hgsl+36Q1poSqpWZ7OCEEmrO7eBANfNC3gJTOPSBig4FXdzqshM9iQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                value: "2d099274-e775-43c3-87a5-0ac562c64c7a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b1a9e08f-2e32-4a98-8310-e65f1ad1be6a", "AQAAAAEAACcQAAAAEHYbw/ZDJqyPKPbaaGnnzl2q7BW/ANN+RoWe1pgUQXohBKCGU9ECEmdD5t+sxNA+rA==" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class LastGeocodeAttempt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "PoliceDispatchCalls",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Accuracy",
                table: "PoliceDispatchCalls",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastGeocodeAttempt",
                table: "PoliceDispatchCalls",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "FireDispatchCalls",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Accuracy",
                table: "FireDispatchCalls",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastGeocodeAttempt",
                table: "FireDispatchCalls",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Accuracy",
                table: "Crimes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastGeocodeAttempt",
                table: "Crimes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Crimes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "a6d0f79d-483c-408d-8b95-da1e5da1e80e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "491e3952-6d52-4b27-a9c1-2ff32ecdef8d", "AQAAAAEAACcQAAAAEFWWqAZbg+hMiAyzp4OFjXAjW2cDYCJ3gKopoFk5dfeqwWwYOXE7S2bfebU8+OxgAA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastGeocodeAttempt",
                table: "PoliceDispatchCalls");

            migrationBuilder.DropColumn(
                name: "LastGeocodeAttempt",
                table: "FireDispatchCalls");

            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "LastGeocodeAttempt",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Crimes");

            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "PoliceDispatchCalls",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Accuracy",
                table: "PoliceDispatchCalls",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "FireDispatchCalls",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Accuracy",
                table: "FireDispatchCalls",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "dd418b15-eb5a-47f6-a15a-b0fa5dd150a9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c99eb1ca-a180-4b7e-986c-fd8927512592", "AQAAAAEAACcQAAAAEJZb/Wiy9xco9RUsPA+ioCQN/ooQiXPe9XlCSMZpfUMHOATTzYuUOyYwKiwjlbUJRQ==" });
        }
    }
}

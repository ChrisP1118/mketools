using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class DispatchCallIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "6f24baa3-26eb-4e21-bd6c-a1eaf8a56968");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "05f3905a-957f-4270-aaf3-a11e7c7c3449", "AQAAAAEAACcQAAAAEH5rga3bMddB0rwoQD7uVhjLS1O8DE/pP9abHT72kfOWb8bFE2+QpBfJQGf6H890sg==" });

            migrationBuilder.CreateIndex(
                name: "IX_PoliceDispatchCalls_ReportedDateTime",
                table: "PoliceDispatchCalls",
                column: "ReportedDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_FireDispatchCalls_ReportedDateTime",
                table: "FireDispatchCalls",
                column: "ReportedDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PoliceDispatchCalls_ReportedDateTime",
                table: "PoliceDispatchCalls");

            migrationBuilder.DropIndex(
                name: "IX_FireDispatchCalls_ReportedDateTime",
                table: "FireDispatchCalls");

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
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class CrimeIndex1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "b9ffd01c-7ab9-4c31-9f03-c529789eb9b4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e1b81862-1257-463c-b233-d0d077f3c9fa", "AQAAAAEAACcQAAAAENP2poRyQuZ31EpYIy6SbDE4Q66kGuuUcoDMbitImD4b9knvWIN/eIk9DEnhqujrOQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Crimes_ReportedDateTime",
                table: "Crimes",
                column: "ReportedDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Crimes_ReportedDateTime",
                table: "Crimes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "bb5cab09-d24c-460d-a849-37253f71e7de");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e0bd83dc-9736-44ea-834e-16903e05eb50", "AQAAAAEAACcQAAAAEOvjnicGG3yY3ccTRIrq1PZ0e58JVBGSPwAuzz/tMJ5HKVxcn23TZBizl6E84zUYKQ==" });
        }
    }
}

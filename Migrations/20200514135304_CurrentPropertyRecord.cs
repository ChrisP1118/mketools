using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class CurrentPropertyRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "ce2d53c7-d50c-4c0d-9744-ed0003adbffe");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b7f778c4-caa2-42b3-aecf-b5c5f28823ca", "AQAAAAEAACcQAAAAEMbcVvowOqyaLesPgLClrVf/Aw3rt3rukkESD802mCmp/zcrbKsnNMMmqqw7Vcq7lA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "89e8d56a-2e94-4048-8ebd-b9d9d7b98094");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "25e92ba4-082a-497a-aeba-4004b1f3d90f", "AQAAAAEAACcQAAAAEE42PQ2QuEw7oyYhpbNoZDVJy/uCLOP9OI5NbD0832PegIkLKgVe7solx0JRQZnIGQ==" });
        }
    }
}

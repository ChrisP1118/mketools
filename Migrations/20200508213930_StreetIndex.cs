using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class StreetIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Streets_DIR_STREET_STTYPE",
                table: "Streets",
                columns: new[] { "DIR", "STREET", "STTYPE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Streets_DIR_STREET_STTYPE",
                table: "Streets");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "13d76b35-3071-4f2a-befb-8f9facfb39fb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d4545db8-a826-48ee-975c-0f2dd35a5478", "AQAAAAEAACcQAAAAEFeoyMEkvOdRgQbZLwQMdXtlulRjGNv/u1mf1Tfe/Wbrw2R72Xa5QGezp/LiGm21FA==" });
        }
    }
}

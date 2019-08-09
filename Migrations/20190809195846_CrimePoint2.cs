using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class CrimePoint2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "17741d36-cc13-45ae-9194-2ba0f7e4460d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cefc8af4-dd76-4e0b-a0ae-84f0fadc7fd3", "AQAAAAEAACcQAAAAECZDF0yakff51rs/8wmxJawv0bm0eESRybdUtxls3E1D8oRaU7nl1dBtY848NasjSQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "6c28982f-1b78-45f1-a060-eafc4d144f63");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5f7aca8d-145b-4c67-8b0b-6b8ad35de8dd", "AQAAAAEAACcQAAAAECINZE6+NC02SzAEzdDKQJ4OGFFZnzuY+TSaLFnHdOsgh19v6jxSeKljz32WY6D/uw==" });
        }
    }
}

using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class CrimePoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IPoint>(
                name: "Point",
                table: "Crimes",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Point",
                table: "Crimes");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "3d9ef2eb-d7ea-41ff-a67d-60693bd2fec4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7313fc0c-afd1-4f17-9b4e-105a8b0a2cfd", "AQAAAAEAACcQAAAAEOBEKbnrjkYVec5jXxlG+aw/ebVrsu4v3qEok1s5AY/bWiRl4H0AYtF8j3fRYqpSiQ==" });
        }
    }
}

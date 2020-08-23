using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace MkeAlerts.Web.Migrations
{
    public partial class HistoricPhotos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoricPhotos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 20, nullable: false),
                    Geometry = table.Column<Geometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    Accuracy = table.Column<int>(nullable: true),
                    Source = table.Column<int>(nullable: true),
                    LastGeocodeAttempt = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<string>(maxLength: 50, nullable: true),
                    Place = table.Column<string>(maxLength: 250, nullable: true),
                    CurrentAddress = table.Column<string>(maxLength: 250, nullable: true),
                    OldAddress = table.Column<string>(maxLength: 250, nullable: true),
                    ImageUrl = table.Column<string>(maxLength: 250, nullable: true),
                    Url = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricPhotos", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "0e5ee386-1e9e-4c33-b5a8-0b4670bdf40a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8271bede-d698-4fca-955d-681cd07edd49", "AQAAAAEAACcQAAAAEO5rPYImDK0lATSnw9aiwNi52VQcpH3dKMlFAXuGICxu0ZInOmQRTN/OD1lVQxB9CQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricPhotos");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "62a1a846-6c2f-4b7c-94ab-5a64a86e84ff");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e1fa8d31-aa5b-4da3-a57c-b272ddf17cd2", "AQAAAAEAACcQAAAAECdts98lFSHqKu9+Slp3JXnaHf0x/wexEgUU3MwzUUVoqFL/ZrsP6SibcrSsyYZtJg==" });
        }
    }
}

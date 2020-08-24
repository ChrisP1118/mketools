using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace MkeAlerts.Web.Migrations
{
    public partial class HistoricPhotoLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accuracy",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "Geometry",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "LastGeocodeAttempt",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "MaxLat",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "MaxLng",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "MinLat",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "MinLng",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "HistoricPhotos");

            migrationBuilder.AddColumn<Guid>(
                name: "HistoricPhotoLocationId",
                table: "HistoricPhotos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HistoricPhotoLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Geometry = table.Column<Geometry>(nullable: true),
                    MinLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLat = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MinLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    MaxLng = table.Column<decimal>(type: "decimal(13, 10)", nullable: false),
                    Accuracy = table.Column<int>(nullable: true),
                    Source = table.Column<int>(nullable: true),
                    LastGeocodeAttempt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricPhotoLocations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "00db390d-6181-44ab-9697-df174beee575");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "856a96ba-3b03-4ea9-8bd9-70d5ae15aa43", "AQAAAAEAACcQAAAAEHljcP9k9b3fDuh/f+byt4OrG95sHktMlMIaWpPuAcChB83unfgG+7dWU0O6DC1n/Q==" });

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhotos_HistoricPhotoLocationId",
                table: "HistoricPhotos",
                column: "HistoricPhotoLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhotoLocations_MaxLat",
                table: "HistoricPhotoLocations",
                column: "MaxLat");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhotoLocations_MaxLng",
                table: "HistoricPhotoLocations",
                column: "MaxLng");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhotoLocations_MinLat",
                table: "HistoricPhotoLocations",
                column: "MinLat");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricPhotoLocations_MinLng",
                table: "HistoricPhotoLocations",
                column: "MinLng");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricPhotos_HistoricPhotoLocations_HistoricPhotoLocationId",
                table: "HistoricPhotos",
                column: "HistoricPhotoLocationId",
                principalTable: "HistoricPhotoLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricPhotos_HistoricPhotoLocations_HistoricPhotoLocationId",
                table: "HistoricPhotos");

            migrationBuilder.DropTable(
                name: "HistoricPhotoLocations");

            migrationBuilder.DropIndex(
                name: "IX_HistoricPhotos_HistoricPhotoLocationId",
                table: "HistoricPhotos");

            migrationBuilder.DropColumn(
                name: "HistoricPhotoLocationId",
                table: "HistoricPhotos");

            migrationBuilder.AddColumn<int>(
                name: "Accuracy",
                table: "HistoricPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Geometry>(
                name: "Geometry",
                table: "HistoricPhotos",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastGeocodeAttempt",
                table: "HistoricPhotos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxLat",
                table: "HistoricPhotos",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxLng",
                table: "HistoricPhotos",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLat",
                table: "HistoricPhotos",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinLng",
                table: "HistoricPhotos",
                type: "decimal(13, 10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "HistoricPhotos",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "f367bc10-39b7-4039-aede-808be849108c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "69bf8f79-afb1-484f-878d-fdfb7d26c19b", "AQAAAAEAACcQAAAAEJ6ZHuKZgyd07ccZnYItkHG4Ix6PENDBjfQqyvjTo3bH0dvShvtzZVlxE4sDK2rbCw==" });
        }
    }
}

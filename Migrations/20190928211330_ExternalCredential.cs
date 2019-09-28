using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class ExternalCredential : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalCredential",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    Provider = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalCredential_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "d1522a92-7fcd-4351-a8b8-8b07511ff839");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c3b753f6-0fc2-4f73-9f02-fca0fc3c7c4b", "AQAAAAEAACcQAAAAEMb7h1sfJTQ5vsa+CgI8I4ndAo0dTOa3XF22OxM3LHLNo3sNz/G0zEz5/cWLaRKipQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalCredential_ApplicationUserId",
                table: "ExternalCredential",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalCredential");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "18f9751b-34d1-4430-be27-35780f48ddd4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "331700ce-8fa5-4d4a-82db-f8ec44c629e1", "AQAAAAEAACcQAAAAEEoiyZhGY8oXMNAAxh9h5IMKpcdjIV7Awg3G0O0zvW6AKeK3epjMEAxElsX/Mr4jUA==" });
        }
    }
}

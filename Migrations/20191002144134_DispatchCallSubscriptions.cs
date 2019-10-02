using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class DispatchCallSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispatchCallSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    DispatchCallType = table.Column<int>(nullable: false),
                    Point = table.Column<IPoint>(nullable: true),
                    SDIR = table.Column<string>(maxLength: 1, nullable: true),
                    STREET = table.Column<string>(maxLength: 18, nullable: true),
                    STTYPE = table.Column<string>(maxLength: 2, nullable: true),
                    HOUSE_NR = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchCallSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchCallSubscriptions_AspNetUsers_ApplicationUserId",
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
                value: "b63fe433-1bdd-4fd5-914a-bf257519bfc6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "5d4a5986-2304-4698-bdb9-762bf0421df4", "admin@mkealerts.com", "admin@mkealerts.com", "admin@mkealerts.com", "AQAAAAEAACcQAAAAEFL29TAc0yVs99SYPmVzO9eQOU1jzmTB47NdyXH+UjLIo9bS/u70Qy25JhvHeWwArA==", "admin@mkealerts.com" });

            migrationBuilder.CreateIndex(
                name: "IX_DispatchCallSubscriptions_ApplicationUserId",
                table: "DispatchCallSubscriptions",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispatchCallSubscriptions");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "1040f4cd-80aa-4c04-8af4-cddb3b3ec498");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "7b51c078-1d16-450c-8eb8-99b1619d26a0", "siteadmin@test.com", "siteadmin@test.com", "siteadmin", "AQAAAAEAACcQAAAAEDERTAbDI9QJoe493YJIRHrpdnFyE7ANnEAYRkT6ma76HIypyqEHN/zYvKrbFbWnKQ==", "siteadmin" });
        }
    }
}

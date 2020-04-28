using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class PickupDatesSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickupDateSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    HoursBefore = table.Column<int>(nullable: false),
                    NextGarbagePickupDate = table.Column<DateTime>(nullable: true),
                    NextRecyclingPickupDate = table.Column<DateTime>(nullable: true),
                    NextGarbagePickupNotification = table.Column<DateTime>(nullable: true),
                    NextRecyclingPickupNotification = table.Column<DateTime>(nullable: true),
                    LADDR = table.Column<string>(maxLength: 10, nullable: true),
                    SDIR = table.Column<string>(maxLength: 1, nullable: true),
                    SNAME = table.Column<string>(maxLength: 18, nullable: true),
                    STYPE = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupDateSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupDateSubscriptions_AspNetUsers_ApplicationUserId",
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
                value: "b86f95fd-a5ea-4b74-b528-ec706bbbf59b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f4e43fdb-52b6-4bf9-9a34-6a8414aa433b", "AQAAAAEAACcQAAAAEAuk8YFjB6DMq7fJrE+HfPqELpaTTZmmhz/28LbDSYf1sxvWSJjsvIb9TgnkrtYQiQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_PickupDateSubscriptions_ApplicationUserId",
                table: "PickupDateSubscriptions",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickupDateSubscriptions");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "f2f1cdac-0b2c-4c9e-9d4a-323b91093eff");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a91c71e3-5e20-4cd8-b7e6-0ada73835ca3", "AQAAAAEAACcQAAAAEPel9MEt3DaAcAKioAAbNNH6U+Z5tcn1geGuxpGitAEkVSSK+tGzW8b6Utmvua8olw==" });
        }
    }
}

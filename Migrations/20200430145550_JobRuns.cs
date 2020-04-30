using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class JobRuns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JobName = table.Column<string>(maxLength: 40, nullable: true),
                    StartTime = table.Column<DateTimeOffset>(nullable: false),
                    EndTime = table.Column<DateTimeOffset>(nullable: true),
                    SuccessCount = table.Column<int>(nullable: false),
                    FailureCount = table.Column<int>(nullable: false),
                    ErrorMessages = table.Column<string>(nullable: true),
                    ErrorStackTrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRuns", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "1544a9f3-f3ca-4b8c-b1c2-c1721a75be67");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "87795195-30f9-42d4-8d71-26cd8e580baa", "AQAAAAEAACcQAAAAEFw2lO9JRD+ep3+v+A6lsB25ixEFmEtfk6szdCc7f8Oqhf4y+VoLd2O+Sani672BPQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobRuns");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "59589b40-bfd3-4db6-8cb6-d5c6442b850c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dcc9c42f-78fa-4753-9edf-f5baab2adb3a", "AQAAAAEAACcQAAAAEHkM3Tbv0Nd5gs62b3ZVTE4NAqWh/xtFjcUAfXiDtiaBpjTy+h522CfTLnCxGR8kag==" });
        }
    }
}

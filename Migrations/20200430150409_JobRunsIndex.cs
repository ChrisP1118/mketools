using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class JobRunsIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "71c40c78-0c61-43df-83e7-cb1b9e632474");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0fd0740c-fce2-4b8c-b931-399f050cb151", "AQAAAAEAACcQAAAAEG6YLwdEWtIWX9wYQznyMXz5hIF7YBh8VQSKAyARIVkwSQPSv7NHbitOuaaeo2EwDg==" });

            migrationBuilder.CreateIndex(
                name: "IX_JobRuns_StartTime",
                table: "JobRuns",
                column: "StartTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobRuns_StartTime",
                table: "JobRuns");

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
    }
}

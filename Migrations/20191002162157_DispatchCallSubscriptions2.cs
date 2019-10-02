using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeAlerts.Web.Migrations
{
    public partial class DispatchCallSubscriptions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Distance",
                table: "DispatchCallSubscriptions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "11d8b4ca-09b5-4794-a76d-138bac214ee2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9e6ef23d-c437-4083-9309-c0c23600eca6", "AQAAAAEAACcQAAAAELufMJGyHbs8PJyiaTjaKEpQILZfWbjII4wNv7nM1CiIVZEnGUkKFGD4oivzczY7Pg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "DispatchCallSubscriptions");

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5d4a5986-2304-4698-bdb9-762bf0421df4", "AQAAAAEAACcQAAAAEFL29TAc0yVs99SYPmVzO9eQOU1jzmTB47NdyXH+UjLIo9bS/u70Qy25JhvHeWwArA==" });
        }
    }
}

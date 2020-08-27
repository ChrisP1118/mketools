using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MkeTools.Web.Migrations
{
    public partial class PropertyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Parcels_TAXKEY",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.AlterColumn<string>(
                name: "TAXKEY",
                table: "Properties",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Properties",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SourceDate",
                table: "Properties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "a09bf2be-7b0b-484a-91b0-375794e79457");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f1ee38ab-c196-4429-a2cd-9d575e6b560c", "AQAAAAEAACcQAAAAEGaHlTBP4+iH+4glqZa7jZAqCtRmjbqomtl53Lx5xMklxWgMr0tFiRFzp5qGYA8OoA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_TAXKEY",
                table: "Properties",
                column: "TAXKEY");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Parcels_TAXKEY",
                table: "Properties",
                column: "TAXKEY",
                principalTable: "Parcels",
                principalColumn: "TAXKEY",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Parcels_TAXKEY",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_TAXKEY",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "SourceDate",
                table: "Properties");

            migrationBuilder.AlterColumn<string>(
                name: "TAXKEY",
                table: "Properties",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "TAXKEY");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e3f1477-2377-4e5f-b02c-a13b9795e157"),
                column: "ConcurrencyStamp",
                value: "8fe6ac9b-4954-46d1-8e66-4dad56e6cfa4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("85f00d40-d578-4988-9f22-4d023175f852"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3cb7c226-5bb3-4cec-8cd6-f4337e4a9fbc", "AQAAAAEAACcQAAAAEBbz+D04htrBzXyhbona/OJbGvhfIP9otU7jIGkv5a3mZfTGL2ezTxvDcwtKGKnRQQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Parcels_TAXKEY",
                table: "Properties",
                column: "TAXKEY",
                principalTable: "Parcels",
                principalColumn: "TAXKEY",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
